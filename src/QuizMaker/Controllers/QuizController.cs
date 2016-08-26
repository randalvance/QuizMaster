using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.Data.Services;
using QuizMaker.Models;
using QuizMaker.Models.QuizViewModels;
using QuizMaker.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext appDbContext;
        private readonly QuizService quizService;

        private readonly UserManager<ApplicationUser> userManager;

        public QuizController(
            ApplicationDbContext appDbContext,
            UserManager<ApplicationUser> userManager,
            QuizService quizService)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.quizService = quizService;
        }

        public IActionResult Index()
        {
            var viewModel = new QuizListViewModel()
            {
                Quizes = appDbContext.Quizes
                        .Include(x => x.QuizGroup)
                        .Include(x => x.QuizQuestions)
                        .OrderByDescending(x => x.ModifyDate)
                        .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Add(Guid groupId, int questionCount = 10)
        {
            var quizGroup = appDbContext.QuizGroups.Include(x => x.Quizes).SingleOrDefault(x => x.QuizGroupId == groupId);
            var quizCount = quizGroup?.Quizes.Count;
            var firstQuiz = quizGroup?.Quizes.FirstOrDefault();
            var quizInstructions = firstQuiz != null ? firstQuiz.Instructions : string.Empty;

            var viewModel = new QuizEditViewModel()
            {
                Code = quizGroup != null ? $"{quizGroup.Code}_{quizCount + 1}" : string.Empty,
                Title = quizGroup != null ? $"{quizGroup.Name} {quizCount + 1}" : string.Empty,
                Instructions = quizInstructions,
                Questions = Enumerable.Range(0, questionCount).Select(i => new QuizEditQuestionViewModel() { }).ToList(), // Hardcoded to 10 questions for now
                Groups = appDbContext.QuizGroups.ToList(),
                QuizGroupId = groupId,
                QuizType = QuizType.FillInTheBlanks
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public  async Task<IActionResult> Add(QuizEditViewModel viewModel)
        {
            return await EditOrAddQuizAsync(viewModel, true);
        }

        public IActionResult Detail(Guid id, string returnUrl)
        {
            var quiz = appDbContext.Quizes
                .Include(x => x.QuizGroup)
                .Include(x => x.QuizChoices)
                .Include(x => x.QuizQuestions)
                .ThenInclude(x => x.Question).ThenInclude(x => x.Answers)
                .SingleOrDefault(q => q.QuizId == id);

            var viewModel = new QuizEditViewModel()
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Code = quiz.Code,
                Instructions = quiz.Instructions,
                QuizType = quiz.QuizType,
                QuizGroupId = quiz.QuizGroupId,
                QuizGroupName = quiz.QuizGroup.Name,
                QuizChoices = quiz.QuizChoices.Count > 0 ? quiz.QuizChoices.Select(c => c.Choice).Aggregate((x, y) => $"{x}:{y}") : string.Empty,
                Questions = quiz.QuizQuestions.Select(x => 
                    new QuizEditQuestionViewModel { QuestionText = x.Question.QuestionText, AnswerData = x.Question.Answers.Select(a => a.AnswerText)
                        .Aggregate((a1, a2) => a1 + ":" + a2) }).ToList(),
                ReadOnly = true
            };

            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", viewModel);
        }

        public IActionResult Edit(Guid id, string returnUrl)
        {
            var quiz = appDbContext.Quizes
                .Include(x => x.QuizGroup)
                .Include(x => x.QuizQuestions).ThenInclude(x => x.Question).ThenInclude(x => x.Answers)
                .SingleOrDefault(q => q.QuizId == id);

            var viewModel = new QuizEditViewModel()
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Code = quiz.Code,
                Instructions = quiz.Instructions,
                QuizType = quiz.QuizType,
                QuizGroupId = quiz.QuizGroupId,
                QuizGroupName = quiz.QuizGroup.Name,
                Questions = quiz.QuizQuestions.Select(x =>
                    new QuizEditQuestionViewModel
                    {
                        QuestionId = x.QuestionId,
                        QuestionText = x.Question.QuestionText,
                        AnswerData = x.Question.Answers.Select(a => a.AnswerText)
                        .Aggregate((a1, a2) => a1 + ":" + a2)
                    }).ToList(),
                Groups = appDbContext.QuizGroups.ToList()
            };

            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuizEditViewModel viewModel)
        {
            return await EditOrAddQuizAsync(viewModel, false);
        }

        public async Task<IActionResult> RecommendedQuiz()
        {
            Guid recommendedQuizId = await GetRecommendedQuizId();

            var quiz = appDbContext.Quizes.SingleOrDefault(x => x.QuizId == recommendedQuizId);
            var viewModel = new RecommendedQuizViewModel() { QuizId = quiz.QuizId, Title = quiz.Title };

            return View(viewModel);
        }

        public async Task<IActionResult> GenerateSession(Guid? id)
        {
            Guid quizId = id.HasValue ? id.Value : await GetRecommendedQuizId();

            var quiz = appDbContext.Quizes.Include(x => x.QuizQuestions).SingleOrDefault(x => x.QuizId == quizId);

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var session = new Session()
            {
                ApplicationUserId = user.Id,
                CorrectAnswerCount = 0,
                QuizItemCount = quiz.QuizQuestions.Count,
                SessionStatus = SessionStatus.Active,
                QuizSessions = new List<QuizSession>()
                {
                    new QuizSession()
                    {
                        QuizId = quizId
                    }
                }
            };

            appDbContext.Sessions.Add(session);
            appDbContext.SaveChanges();

            return RedirectToAction("TakeQuiz", "Quiz", new { sessionId = session.SessionId });
        }

        public IActionResult ShowAnswers(Guid sessionId, bool hideNext, bool firstAnswers)
        {
            Session session = GetSessionForShowingAnswer(sessionId);

            var maxChronology = session.SessionAnswers.Max(x => x.AnswerChronology);

            var sessionAnswers = session.SessionAnswers.Where(x => firstAnswers || maxChronology == 0 ? x.AnswerChronology == 0 : x.AnswerChronology != 0).ToList();

            ShowAnswersViewModel viewModel = GetShowAnswersViewModel(sessionId, hideNext, sessionAnswers);

            return View(viewModel);
        }

        public async Task<IActionResult> TakeQuiz(Guid sessionId)
        {
            Session session = await GetSessionForTakingQuizAsync(sessionId);

            if (session == null)
            {
                return View(new QuizPageViewModel());
            }

            QuizPageViewModel viewModel = GetQuizPageViewModel(session);

            if (session.SessionAnswers.Count > 0)
            {
                await TakeQuizAsync(viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TakeQuiz([FromForm] QuizPageViewModel viewModel)
        {
            await TakeQuizAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var quiz = appDbContext.Quizes.SingleOrDefault(x => x.QuizId == id);
            appDbContext.Remove(quiz);
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { deleteSuccess = true });
        }

        private async Task TakeQuizAsync(QuizPageViewModel viewModel)
        {
            int lastChronologicalOrder = await ClearSessionAnswersAsync(viewModel);

            int correctAnswerCount = 0;

            var sessionAnswers = new List<SessionAnswer>();
            var session = await appDbContext.Sessions.FirstOrDefaultAsync(s => s.SessionId == viewModel.SessionId);
            var quizes = appDbContext.Quizes.Include(x => x.QuizChoices).AsQueryable();

            viewModel.QuizItemCount = 0;

            foreach (var quiz in viewModel.Quizes)
            {
                int quizCorrectAnswerCount = 0;

                quizCorrectAnswerCount = await ProcessQuestionsAsync(viewModel.SessionId, lastChronologicalOrder, sessionAnswers, quiz);

                correctAnswerCount += quizCorrectAnswerCount;

                if (session.SessionStatus == SessionStatus.Active)
                {
                    quiz.CorrectAnswerCount = quizCorrectAnswerCount;
                }
                quiz.QuizItemCount = quiz.Questions.Count;
                viewModel.QuizItemCount += quiz.QuizItemCount;

                var quizObj = quizes.SingleOrDefault(x => x.QuizId == quiz.QuizId);
                quiz.QuizChoices = new SelectList(quizObj.QuizChoices, "Choice", "Choice");
            }

            if (session.SessionStatus == SessionStatus.Active)
            {
                session.CorrectAnswerCount = correctAnswerCount;
                session.SessionStatus = SessionStatus.Done;
                session.DateCompleted = DateTime.Now;
            }

            appDbContext.SessionAnswers.AddRange(sessionAnswers);

            await appDbContext.SaveChangesAsync();

            viewModel.InitialLoad = false;
        }

        private async Task<Guid> GetRecommendedQuizId()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var recommendedQuizId = await quizService.GetRecommendedQuizAsync(user.Id);
            return recommendedQuizId;
        }
        
        private async Task<IActionResult> EditOrAddQuizAsync(QuizEditViewModel viewModel, bool isAdd)
        {
            viewModel.Groups = await appDbContext.QuizGroups.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            var quizChoices = await ParseQuizChoicesAsync(viewModel.QuizChoices);

            Quiz quiz = GetQuizForEditing(viewModel, quizChoices, isAdd);

            UpdateChoices(isAdd, quizChoices, quiz);

            await ProcessQuestionsForEditingAsync(viewModel.Questions, quiz, isAdd);

            if (isAdd)
            {
                appDbContext.Quizes.Add(quiz);
            }

            try
            {
                appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                var exception = ex.InnerException;
                while (exception != null)
                {
                    sb.AppendLine($"{exception.Message} <br />");
                    exception = exception.InnerException;
                }

                ViewBag.ErrorMessage = sb.ToString();
                return View("Edit", viewModel);
            }

            return RedirectToAction("Index", new { addSuccess = true });
        }

        private Task<List<QuizChoice>> ParseQuizChoicesAsync(string quizChoices)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(quizChoices))
                {
                    return new List<QuizChoice>();
                }
                string[] choices = quizChoices.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                return choices.Select((c, i) => new QuizChoice() { Choice = c, DisplayOrder = i * 10 }).ToList();
            });
        }

        private Task<List<Answer>> ParseAnswerDataAsync(string answerData)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(answerData))
                {
                    return new List<Answer>();
                }
                string[] answers = answerData.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                return answers.Select((a, i) => new Answer() { AnswerText = a, OrderNumber = i * 10 }).ToList();
            });
        }
        
        private async Task<int> ClearSessionAnswersAsync(QuizPageViewModel viewModel)
        {
            // Hack, we'll probably find a better way of updating existing session answers
            var sessionAnswers = appDbContext.SessionAnswers.Where(x => x.SessionId == viewModel.SessionId);
            var lastChronologicalOrder = (await sessionAnswers.FirstOrDefaultAsync())?.AnswerChronology + 1 ?? 0;
            var sessionAnswersToDelete = sessionAnswers.Where(x => x.AnswerChronology > 0);
            appDbContext.SessionAnswers.RemoveRange(sessionAnswersToDelete);
            await appDbContext.SaveChangesAsync();

            return lastChronologicalOrder;
        }
        
        private async Task<Session> GetSessionForTakingQuizAsync(Guid sessionId)
        {
            return await appDbContext.Sessions.Include(x => x.SessionAnswers)
                .Include(x => x.QuizSessions)
                    .ThenInclude(x => x.Quiz)
                    .ThenInclude(x => x.QuizQuestions)
                    .ThenInclude(x => x.Question)
                    .ThenInclude(x => x.Answers)
                    .ThenInclude(x => x.SessionAnswers)
                .Include(x => x.QuizSessions)
                    .ThenInclude(x => x.Quiz)
                    .ThenInclude(x => x.QuizChoices)
                .Where(x => x.SessionId == sessionId)
                .FirstOrDefaultAsync();
        }

        private Session GetSessionForShowingAnswer(Guid sessionId)
        {
            return appDbContext.Sessions.Include(s => s.SessionAnswers)
                .ThenInclude(sa => sa.Answer)
                .ThenInclude(a => a.Question)
                .ThenInclude(q => q.QuizQuestions)
                .ThenInclude(qq => qq.Quiz).ToList()
                .SingleOrDefault(x => x.SessionId == sessionId);
        }

        private Quiz GetQuizForEditing(QuizEditViewModel viewModel, List<QuizChoice> quizChoices, bool isAdd)
        {
            return isAdd ? new Quiz()
            {
                Title = viewModel.Title,
                Code = viewModel.Code,
                Instructions = viewModel.Instructions,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                QuizGroupId = viewModel.QuizGroupId,
                QuizType = viewModel.QuizType,
                QuizQuestions = new List<QuizQuestion>(),
                QuizChoices = !string.IsNullOrWhiteSpace(viewModel.QuizChoices) ? quizChoices : new List<QuizChoice>(),
            } : appDbContext.Quizes
                    .Include(x => x.QuizChoices)
                    .Include(x => x.QuizQuestions)
                    .ThenInclude(x => x.Question)
                    .ThenInclude(x => x.Answers)
                    .SingleOrDefault(x => x.QuizId == viewModel.QuizId);
        }

        private static QuizPageViewModel GetQuizPageViewModel(Session session)
        {
            return new QuizPageViewModel()
            {
                SessionId = session.SessionId,
                Quizes = session.QuizSessions.Select(x => x.Quiz)
                         .Select(q => new QuizViewModel()
                         {
                             QuizId = q.QuizId,
                             Title = q.Title,
                             Instructions = q.Instructions,
                             Questions = q.QuizQuestions.Where(qq => qq.QuizId == q.QuizId).Select(qq => new SessionQuestionViewModel()
                             {
                                 QuestionId = qq.QuestionId,
                                 QuestionText = qq.Question.QuestionText,
                                 Answers = qq.Question.Answers.OrderBy(a => a.OrderNumber).Select(answer => new SessionAnswerViewModel()
                                 {
                                     AnswerId = answer.AnswerId,
                                     Order = answer.OrderNumber,
                                     CorrectAnswer = answer.AnswerText,
                                     UserAnswer = answer.SessionAnswers.FirstOrDefault(a => a.SessionId == session.SessionId) != null ?
                                        answer.SessionAnswers.OrderByDescending(x => x.AnswerChronology)
                                        .FirstOrDefault(a => a.SessionId == session.SessionId).UserAnswer : string.Empty
                                 }).ToList()
                             }).ToList(),
                             QuizChoices = new SelectList(q.QuizChoices, "Choice", "Choice")
                         }).ToList(),
                InitialLoad = session.SessionAnswers.Count == 0,
                CorrectAnswerCount = session.CorrectAnswerCount
            };
        }

        private static ShowAnswersViewModel GetShowAnswersViewModel(Guid sessionId, bool hideNext, List<SessionAnswer> sessionAnswers)
        {
            return new ShowAnswersViewModel()
            {
                SessionId = sessionId,
                QuizGroups = sessionAnswers.Select(
                    x =>
                    {
                        var quiz = x.Answer.Question.QuizQuestions.SingleOrDefault(q => q.QuestionId == x.Answer.QuestionId).Quiz;

                        return new SessionAnswerViewModel()
                        {
                            AnswerId = x.AnswerId,
                            QuizId = quiz.QuizId,
                            QuizTitle = quiz.Title,
                            QuizInstructions = quiz.Instructions,
                            QuestionText = x.Answer.Question.QuestionText,
                            CorrectAnswer = x.Answer.AnswerText,
                            UserAnswer = x.UserAnswer
                        };
                    }).GroupBy(savm => savm.QuizId).Select(g => new ShowAnswersQuizGroupViewModel()
                    {
                        QuizTitle = g.FirstOrDefault().QuizTitle,
                        QuizIndustructions = g.FirstOrDefault().QuizInstructions,
                        Answers = g.ToList()
                    }).ToList(),
                HideNext = hideNext
            };
        }

        private static Task<int> ProcessQuestionsAsync(Guid sessionId, int lastChronologicalOrder,
            List<SessionAnswer> sessionAnswers, QuizViewModel quiz)
        {
            return Task.Run(async () =>
            {
                var quizCorrectAnswerCount = 0;

                foreach (var question in quiz.Questions)
                {
                    bool isCorrect = await CheckAnswersAsync(sessionId, quiz, sessionAnswers, 
                        question.Answers, lastChronologicalOrder);

                    quizCorrectAnswerCount += isCorrect ? 1 : 0;
                }

                return quizCorrectAnswerCount;
            });
        }

        private async Task ProcessQuestionsForEditingAsync(List<QuizEditQuestionViewModel> questions, Quiz quiz, bool isAdd)
        {
            foreach (var question in questions)
            {
                var answers = await ParseAnswerDataAsync(question.AnswerData);

                if (isAdd)
                {
                    var newQuestion = new Question();
                    newQuestion.QuestionText = question.QuestionText;
                    newQuestion.Answers = answers;
                    quiz.QuizQuestions.Add(new QuizQuestion() { Question = newQuestion });
                }
                else
                {
                    var questionToUpdate = quiz.QuizQuestions.SingleOrDefault(q => q.QuestionId == question.QuestionId).Question;

                    if (questionToUpdate.QuestionText != question.QuestionText)
                    {
                        questionToUpdate.QuestionText = question.QuestionText;
                    }

                    for (int i = 0; i < answers.Count; i++)
                    {
                        if (i < questionToUpdate.Answers.Count)
                        {
                            var answerToUpdate = questionToUpdate.Answers[i];

                            if (answerToUpdate.AnswerText != answers[i].AnswerText)
                            {
                                answerToUpdate.AnswerText = answers[i].AnswerText;
                            }
                        }
                        else
                        {
                            var newAnswer = new Answer() { AnswerText = answers[i].AnswerText, OrderNumber = i * 10 };
                            questionToUpdate.Answers.Add(newAnswer);
                        }
                    }
                }
            }
        }

        private static Task<bool> CheckAnswersAsync(
            Guid sessionId,
            QuizViewModel quiz,
            List<SessionAnswer> sessionAnswers,
            List<SessionAnswerViewModel> questionAnswers, 
            int lastChronologicalOrder)
        {
            return Task.Run(() =>
            {
                var correctAnswers = questionAnswers.Select(answer => answer.CorrectAnswer).ToList();

                // Check for incorrectness
                foreach (var answer in questionAnswers)
                {
                    var sessionAnswer = new SessionAnswer()
                    {
                        SessionId = sessionId,
                        UserAnswer = answer.UserAnswer,
                        AnswerId = answer.AnswerId,
                        AnswerChronology = lastChronologicalOrder
                    };

                    sessionAnswers.Add(sessionAnswer);

                    if (string.IsNullOrWhiteSpace(answer.UserAnswer) ||
                        (quiz.AnswersOrderImportant && answer.CorrectAnswer.ToLower() != answer.UserAnswer.ToLower().Trim()) ||
                        (correctAnswers.Exists(a => a.ToLower() == answer.UserAnswer.ToLower().Trim())))
                    {
                        return MarkAnswerAsIncorrect(quiz, answer, sessionAnswer);
                    }
                }

                return true;
            });
        }

        private static void UpdateChoices(bool isAdd, List<QuizChoice> quizChoices, Quiz quizzy)
        {
            if (!isAdd)
            {
                for (int i = 0; i < quizChoices.Count; i++)
                {
                    if (i < quizzy.QuizChoices.Count)
                    {
                        var choiceToUpdate = quizzy.QuizChoices[i];

                        if (choiceToUpdate.Choice != quizChoices[i].Choice)
                        {
                            choiceToUpdate.Choice = quizChoices[i].Choice;
                        }
                    }
                    else
                    {
                        var newChoice = new QuizChoice() { Choice = quizChoices[i].Choice, DisplayOrder = i * 10 };
                        quizzy.QuizChoices.Add(newChoice);
                    }
                }
            }
        }

        private static bool MarkAnswerAsIncorrect(QuizViewModel quiz, SessionAnswerViewModel answer, SessionAnswer sessionAnswer)
        {
            quiz.IncorrectAnswers.Add(answer.AnswerId);
            return sessionAnswer.IsCorrect = false;
        }
    }
}
