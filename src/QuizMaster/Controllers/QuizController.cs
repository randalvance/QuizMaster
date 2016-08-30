using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Core;
using QuizMaker.Data.Repositories;
using QuizMaster.Data;
using QuizMaster.Data.Services;
using QuizMaster.Data.Settings;
using QuizMaster.Models;
using QuizMaster.Models.QuizViewModels;
using QuizMaster.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly QuizGroupRepository quizGroupRepository;
        private readonly QuizRepository quizRepository;
        private readonly QuizService quizService;
        private readonly QuizSettings quizSettings;
        private readonly SessionAnswerRepository sessionAnswerRepository;
        private readonly SessionSettings sessionSettings;
        private readonly SessionRepository sessionRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public QuizController(
            QuizGroupRepository quizGroupRepository,
            QuizRepository quizRepository,
            QuizService quizService,
            QuizSettings quizSettings,
            SessionAnswerRepository sessionAnswerRepository,
            SessionSettings sessionSettings, 
            SessionRepository sessionRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.quizGroupRepository = quizGroupRepository;
            this.quizRepository = quizRepository;
            this.quizService = quizService;
            this.quizSettings = quizSettings;
            this.sessionAnswerRepository = sessionAnswerRepository;
            this.sessionSettings = sessionSettings;
            this.sessionRepository = sessionRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new QuizListViewModel()
            {
                Quizes = await quizRepository.RetrieveAll(new ListOptions<Quiz>(
                        x => x.QuizGroup,
                        x => x.QuizQuestions))
                        .OrderByDescending(x => x.ModifyDate).ToListAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Add(Guid groupId, int questionCount = 10, string returnUrl = "")
        {
            var quizGroup = await quizGroupRepository.RetrieveAsync(groupId, new ListOptions<QuizGroup>(
                        x => x.Quizes));
            var quizCount = quizGroup?.Quizes.Count;
            var firstQuiz = quizGroup?.Quizes.FirstOrDefault();
            var quizInstructions = firstQuiz != null ? firstQuiz.Instructions : string.Empty;

            var viewModel = new QuizEditViewModel()
            {
                Code = quizGroup != null ? $"{quizGroup.Code}_{quizCount + 1}" : string.Empty,
                Title = quizGroup != null ? $"{quizGroup.Name} {quizCount + 1}" : string.Empty,
                Instructions = quizInstructions,
                Questions = Enumerable.Range(0, questionCount).Select(i => new QuizEditQuestionViewModel() { }).ToList(), // Hardcoded to 10 questions for now
                Groups = quizGroupRepository.RetrieveAll().ToList(),
                QuizGroupId = groupId,
                QuizType = QuizType.FillInTheBlanks,
                ReturnUrl = returnUrl
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public  async Task<IActionResult> Add(QuizEditViewModel viewModel)
        {
            return await EditOrAddQuizAsync(viewModel, true);
        }

        public async Task<IActionResult> Detail(Guid id, string returnUrl)
        {
            var quiz = 
                await quizRepository.RetrieveAsync(id, new ListOptions<Quiz>(
                    x => x.QuizGroup,
                    x => x.QuizChoices,
                    x => x.QuizQuestions[0].Question.Answers));

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
                ReadOnly = true,
                ReturnUrl = returnUrl
            };

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Edit(Guid id, string returnUrl)
        {
            var quiz =
                await quizRepository.RetrieveAsync(id, new ListOptions<Quiz>(
                    x => x.QuizGroup,
                    x => x.QuizQuestions[0].Question.Answers));

            var viewModel = new QuizEditViewModel()
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Code = quiz.Code,
                Instructions = quiz.Instructions,
                QuizType = quiz.QuizType,
                QuizGroupId = quiz.QuizGroupId,
                QuizGroupName = quiz.QuizGroup.Name,
                Questions = quiz.QuizQuestions?.Select(x =>
                    new QuizEditQuestionViewModel
                    {
                        QuestionId = x.QuestionId,
                        QuestionText = x.Question.QuestionText,
                        AnswerData = x.Question.Answers.Select(a => a.AnswerText)?
                        .Aggregate((a1, a2) => a1 + ":" + a2)
                    }).ToList(),
                Groups = quizGroupRepository.RetrieveAll().ToList()
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
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Guid recommendedQuizId = await quizService.GetRecommendedQuizAsync(user.Id);

            var quiz = await quizRepository.RetrieveAsync(recommendedQuizId);

            if (quiz == null)
            {
                return RedirectToAction("NoMoreQuiz");
            }

            var viewModel = new RecommendedQuizViewModel() { QuizId = quiz.QuizId, Title = quiz.Title };

            return View(viewModel);
        }

        public async Task<IActionResult> TakeQuiz(Guid sessionId, bool hideNext = false)
        {
            Session session = await GetSessionForTakingQuizAsync(sessionId);

            if (session == null)
            {
                return View(new QuizPageViewModel());
            }

            QuizPageViewModel viewModel = GetQuizPageViewModel(session);

            await RandomizeQuestionDisplayOrdersAsync(viewModel, session);
            SortQuestions(viewModel);

            viewModel.QuizOfTheDayNumber = await quizService.GetQuizOfTheDaySequenceNumberAsync(User);

            if (session.SessionAnswers.Count > 0)
            {
                await TakeQuizAsync(viewModel, false);
            }

            ViewBag.HideNext = hideNext && session.SessionStatus == SessionStatus.Done;

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
            var quiz = await quizRepository.RetrieveAsync(id);
            await quizRepository.RemoveAsync(quiz);
            await quizRepository.CommitAsync();

            return RedirectToAction("Index", new { deleteSuccess = true });
        }

        public IActionResult NoMoreQuiz()
        {
            return View();
        }

        private async Task TakeQuizAsync(QuizPageViewModel viewModel, bool isPost = true)
        {
            int lastChronologicalOrder = await GetLastChronologicalOrder(viewModel.SessionId);
            viewModel.IsRetry = sessionAnswerRepository.RetrieveAll().Any(x => x.SessionId == viewModel.SessionId);

            if (isPost)
            {
                await ClearSessionAnswersAsync(viewModel);
            }

            int correctAnswerCount = 0;

            var sessionAnswers = new List<SessionAnswer>();
            var session = await sessionRepository.RetrieveAsync(viewModel.SessionId);
            var quizes = quizRepository.RetrieveAll(new ListOptions<Quiz>(x => x.QuizChoices)).AsQueryable();

            viewModel.QuizItemCount = 0;
            viewModel.CorrectAnswerCount = session.CorrectAnswerCount;

            foreach (var quiz in viewModel.Quizes)
            {
                int quizCorrectAnswerCount = 0;

                quizCorrectAnswerCount = await ProcessQuestionsAsync(viewModel.SessionId, lastChronologicalOrder, sessionAnswers, quiz);

                correctAnswerCount += quizCorrectAnswerCount;

                if (session.SessionStatus == SessionStatus.Ongoing)
                {
                    quiz.CorrectAnswerCount = quizCorrectAnswerCount;
                }
                quiz.QuizItemCount = quiz.Questions.Count;
                viewModel.QuizItemCount += quiz.QuizItemCount;

                var quizObj = quizes.SingleOrDefault(x => x.QuizId == quiz.QuizId);
                quiz.QuizChoices = new SelectList(quizObj.QuizChoices, "Choice", "Choice");
            }

            if (session.SessionStatus == SessionStatus.Ongoing && isPost)
            {
                session.CorrectAnswerCount = correctAnswerCount;
                session.SessionStatus = SessionStatus.Done;
                session.DateCompleted = DateTime.Now;

                viewModel.CorrectAnswerCount = correctAnswerCount;
            }

            viewModel.RetryAnswerCount = correctAnswerCount;
            
            if (isPost)
            {
                await sessionAnswerRepository.AddRangeAsync(sessionAnswers);
                await sessionAnswerRepository.CommitAsync();
            }

            viewModel.InitialLoad = false;
        }
        
        private async Task<IActionResult> EditOrAddQuizAsync(QuizEditViewModel viewModel, bool isAdd)
        {
            viewModel.Groups = quizGroupRepository.RetrieveAll().ToList();

            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            var quizChoices = await ParseQuizChoicesAsync(viewModel.QuizChoices);

            Quiz quiz = await GetQuizForEditingAsync(viewModel, quizChoices, isAdd);

            if (!isAdd)
            {
                quiz.Code = viewModel.Code;
                quiz.Title = viewModel.Title;
                quiz.Instructions = viewModel.Instructions;
                quiz.QuizType = viewModel.QuizType;
                quiz.QuizGroupId = viewModel.QuizGroupId;
                quiz.ModifyDate = DateTime.Now;
            }

            UpdateChoices(isAdd, quizChoices, quiz);

            await ProcessQuestionsForEditingAsync(viewModel.Questions, quiz, isAdd);

            if (isAdd)
            {
                await quizRepository.AddAsync(quiz);
            }

            try
            {
                await quizRepository.CommitAsync();
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

            if (!string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
            {
                return Redirect(viewModel.ReturnUrl);
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
        
        private async Task ClearSessionAnswersAsync(QuizPageViewModel viewModel)
        {
            // Hack, we'll probably find a better way of updating existing session answers
            var sessionAnswers = (await sessionAnswerRepository.RetrievAllAsync()).Where(x => x.SessionId == viewModel.SessionId);
            var sessionAnswersToDelete = sessionAnswers.Where(x => x.AnswerChronology > 0);
            await sessionAnswerRepository.RemoveRangeAsync(sessionAnswersToDelete);
            await sessionAnswerRepository.CommitAsync();
        }

        private async Task<int> GetLastChronologicalOrder(Guid sessionId)
        {
            var firstSessionAnswer = await sessionAnswerRepository.RetrieveAll().FirstOrDefaultAsync(x => x.SessionId == sessionId);
            var lastChronologicalOrder = firstSessionAnswer?.AnswerChronology + 1 ?? 0;
            return lastChronologicalOrder;
        }
        
        private async Task<Session> GetSessionForTakingQuizAsync(Guid sessionId)
        {
            return await sessionRepository.RetrieveAll(new ListOptions<Session>(
                x => x.SessionAnswers,
                x => x.QuizSessions[0].Quiz.QuizQuestions[0].Question.Answers[0].SessionAnswers,
                x => x.QuizSessions[0].Quiz.QuizChoices,
                x => x.SessionQuestions))
                .Where(x => x.SessionId == sessionId)
                .FirstOrDefaultAsync();
        }
        
        private async Task<Quiz> GetQuizForEditingAsync(QuizEditViewModel viewModel, List<QuizChoice> quizChoices, bool isAdd)
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
            } : await quizRepository.RetrieveAsync(viewModel.QuizId, new ListOptions<Quiz>(
                    x => x.QuizChoices,
                    x => x.QuizQuestions[0].Question.Answers));
        }

        private static QuizPageViewModel GetQuizPageViewModel(Session session)
        {
            var viewModel = new QuizPageViewModel()
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

            return viewModel;
        }

        private async Task RandomizeQuestionDisplayOrdersAsync(QuizPageViewModel viewModel, Session session)
        {
            var randomizer = new Random();
            
            foreach(var quiz in viewModel.Quizes)
            {
                var displayOrders = Enumerable.Range(1, quiz.Questions.Count).ToList();
                foreach(var question in quiz.Questions)
                {
                    var randomIndex = randomizer.Next(0, displayOrders.Count - 1);
                    var randomDisplayOrder = displayOrders[randomIndex];

                    var sessionQuestion = session.SessionQuestions.SingleOrDefault(x => x.QuestionId == question.QuestionId);

                    if (sessionQuestion == null)
                    {
                        sessionQuestion = new SessionQuestion()
                        {
                            QuestionId = question.QuestionId,
                            DisplayOrder = randomDisplayOrder
                        };

                        session.SessionQuestions.Add(sessionQuestion);
                        displayOrders.Remove(randomDisplayOrder);
                    }
                    else
                    {
                        displayOrders.Remove(sessionQuestion.DisplayOrder);
                    }

                    question.DisplayOrder = sessionQuestion.DisplayOrder;
                }
            }

            await sessionRepository.CommitAsync();
        }

        private Task<int> ProcessQuestionsAsync(Guid sessionId, int lastChronologicalOrder,
            List<SessionAnswer> sessionAnswers, QuizViewModel quiz)
        {
            return Task.Run(async () =>
            {
                var quizCorrectAnswerCount = 0;

                foreach (var question in quiz.Questions)
                {
                    var isCorrect = await CheckAnswersAsync(sessionId, quiz, sessionAnswers, 
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

        private Task<bool> CheckAnswersAsync(
            Guid sessionId,
            QuizViewModel quiz,
            List<SessionAnswer> sessionAnswers,
            List<SessionAnswerViewModel> questionAnswers, 
            int lastChronologicalOrder)
        {
            return Task.Run(() =>
            {
                var correctAnswers = questionAnswers.Select(answer => answer.CorrectAnswer).ToList();
                var isCorrect = true;

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
                        (quiz.AnswersOrderImportant && answer.CorrectAnswer.ToLower().Trim() != answer.UserAnswer.ToLower().Trim()) ||
                        (!correctAnswers.Exists(a => a.ToLower().Trim() == answer.UserAnswer.ToLower().Trim())))
                    {
                        MarkAnswerAsIncorrect(quiz, answer, sessionAnswer);
                        isCorrect = false;
                    }
                    else
                    {
                        sessionAnswer.IsCorrect = true;
                    }
                }
                return isCorrect;
            });
        }

        private void UpdateChoices(bool isAdd, List<QuizChoice> quizChoices, Quiz quizzy)
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

        private void MarkAnswerAsIncorrect(QuizViewModel quiz, SessionAnswerViewModel answer, SessionAnswer sessionAnswer)
        {
            quiz.IncorrectAnswers.Add(answer.AnswerId);
            sessionAnswer.IsCorrect = false;
        }

        private void SortQuestions(QuizPageViewModel viewModel)
        {
            foreach (var quiz in viewModel.Quizes)
            {
                quiz.Questions = quiz.Questions.OrderBy(x => x.DisplayOrder).ToList();
            }
        }
    }
}
