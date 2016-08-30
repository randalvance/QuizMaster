using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Core;
using QuizMaker.Data.Repositories;
using QuizMaster.Data.Constants;
using QuizMaster.Data.Services;
using QuizMaster.Data.Settings;
using QuizMaster.Models;
using QuizMaster.Models.QuizViewModels;
using QuizMaster.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ApplicationSettingService appSettingsService;
        private readonly QuizRepository quizRepository;
        private readonly QuizService quizService;
        private readonly QuizSettings quizSettings;
        private readonly SessionRepository sessionRepository;
        private readonly SessionSettings sessionSettings;
        private readonly UserManager<ApplicationUser> userManager;

        public SessionController(ApplicationSettingService appSettingsService,
                                 QuizRepository quizRepository,
                                 QuizService quizService,
                                 QuizSettings quizSettings,
                                 SessionRepository sessionRepository,
                                 SessionSettings sessionSettings,
                                 UserManager<ApplicationUser> userManager)
        {
            this.appSettingsService = appSettingsService;
            this.quizRepository = quizRepository;
            this.quizService = quizService;
            this.quizSettings = quizSettings;
            this.sessionSettings = sessionSettings;
            this.sessionRepository = sessionRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(Guid? userId = null)
        {
            var isAdmin = User.IsInRole(IdentityConstants.SuperAdministratorRoleName);

            if (userId == null && !isAdmin)
            {
                throw new UnauthorizedAccessException("You are not an administrator.");
            }

            var sessions = await sessionRepository.RetrieveAll(new ListOptions<Session>(
                s => s.ApplicationUser,
                s => s.QuizSessions[0].Quiz
            )).Where(s => (s.DateCompleted.HasValue ? s.DateCompleted.Value : s.DateTaken).Date == DateTime.Now.Date &&
                            ((userId.HasValue && s.ApplicationUserId == userId) || isAdmin))
            .OrderBy(s => s.SessionStatus).ThenByDescending(s => (s.DateCompleted.HasValue ? s.DateCompleted.Value : s.DateTaken))
            .Select(s => new SessionViewModel()
            {
                SessionId = s.SessionId,
                UserName = s.ApplicationUser.UserName,
                SessionStatus = s.SessionStatus.ToString(),
                DateTaken = s.DateTaken,
                DateCompleted = s.DateCompleted,
                CorrectAnswerCount = s.CorrectAnswerCount,
                QuizItemCount = s.QuizItemCount,
                QuizTitle = string.Join("|", s.QuizSessions.Select(qss => qss.Quiz.Title).ToArray())
            }).ToListAsync();

            var passingGrade = await quizSettings.PassingGrade;

            var viewModel = new SessionListViewModel()
            {
                Sessions = sessions,
                UserSpecified = userId.HasValue,
                PassingGrade = passingGrade,
                QuizesCompleted = await quizService.GetQuizOfTheDaySequenceNumberAsync(User) - 1,
                QuizesPassed = sessions.Count(x => x.GradePercentage >= passingGrade),
                QuizesFailed = sessions.Count(x => x.GradePercentage < passingGrade),
                RequiredQuizes = await sessionSettings.RecommendedSessionCountPerDay
            };
            return View(viewModel);
        }
        
        public async Task<IActionResult> SkipSession(Guid sessionId)
        {
            var session = await sessionRepository.RetrieveAsync(sessionId);
            session.SessionStatus = SessionStatus.Skipped;
            await sessionRepository.UpdateAsync(session);
            await sessionRepository.CommitAsync();

            return await GenerateSession();
        }

        public async Task<IActionResult> GenerateSession(Guid? id = null, int numQuizTaken = 0, bool fromTaker = false)
        {
            Guid quizId;
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (id.HasValue)
            {
                quizId = id.Value;
            }
            else
            {
                var recommendedQuizId = await quizService.GetRecommendedQuizAsync(user.Id);
                quizId = recommendedQuizId;
            }

            var quiz = await quizRepository.RetrieveAsync(quizId, new ListOptions<Quiz>(x => x.QuizQuestions));

            if (quiz == null)
            {
                return RedirectToAction("NoMoreQuiz");
            }
            
            var session = new Session()
            {
                ApplicationUserId = user.Id,
                CorrectAnswerCount = 0,
                QuizItemCount = quiz.QuizQuestions.Count,
                SessionStatus = SessionStatus.Ongoing,
                QuizSessions = new List<QuizSession>()
                {
                    new QuizSession()
                    {
                        QuizId = quizId
                    }
                }
            };

            await sessionRepository.AddAsync(session);
            await sessionRepository.CommitAsync();

            if (fromTaker)
            {
                int requiredSessionsPerDay = await sessionSettings.RecommendedSessionCountPerDay;

                if (requiredSessionsPerDay == numQuizTaken)
                {
                    return RedirectToAction("Index", "Session", new { userId = user.Id });
                }
            }
            return RedirectToAction("TakeQuiz", "Quiz", new { sessionId = session.SessionId });
        }
        
        public async Task<IActionResult> ShowAnswers(Guid sessionId, bool hideNext, bool firstAnswers)
        {
            var session = await GetSessionForShowingAnswerAsync(sessionId);

            var maxChronology = session.SessionAnswers != null && session.SessionAnswers.Any() ? session.SessionAnswers.Max(x => x.AnswerChronology) : 0;

            var viewModel = GetShowAnswersViewModel(session, hideNext, firstAnswers, maxChronology);

            return View(viewModel);
        }

        private ShowAnswersPageViewModel GetShowAnswersViewModel(Session session, bool hideNext, bool firstAnswers, int maxChronology)
        {
            int displayOrder = 1;

            Func<Question, int> getDisplayOrder = question =>
            {
                var sessionQuestion = session.SessionQuestions.SingleOrDefault(q => q.QuestionId == question.QuestionId);

                return sessionQuestion != null ? sessionQuestion.DisplayOrder : displayOrder++;
            };

            Func<Answer, ShowAnswersAnswerViewModel> getAnswerViewModel = answer =>
            {
                var sessionAnswer = session.SessionAnswers
                    .Where(x => firstAnswers || maxChronology == 0 ? x.AnswerChronology == 0 : x.AnswerChronology != 0)
                    .FirstOrDefault(sa => sa.AnswerId == answer.AnswerId);

                return sessionAnswer != null ? new ShowAnswersAnswerViewModel()
                {
                    AnswerId = answer.AnswerId,
                    IsCorrect = sessionAnswer.IsCorrect,
                    CorrectAnswer = answer.AnswerText,
                    UserAnswer = sessionAnswer.UserAnswer
                } : new ShowAnswersAnswerViewModel();
            };

            var viewModel = new ShowAnswersPageViewModel()
            {
                SessionId = session.SessionId,
                Quizes = session.QuizSessions
                        .Select(qs => qs.Quiz)
                        .Select(q => new ShowAnswersQuizViewModel()
                        {
                            QuizId = q.QuizId,
                            QuizTitle = q.Title,
                            QuizInstructions = q.Instructions,
                            AnswersOrderImportant = q.AnswersOrderImportant,
                            Questions = q.QuizQuestions.Select(qq => new ShowAnswersQuestionViewModel()
                            {
                                QuestionId = qq.QuestionId,
                                QuestionText = qq.Question.QuestionText,
                                DisplayOrder = getDisplayOrder(qq.Question),
                                Answers = qq.Question.Answers.Select(answer => getAnswerViewModel(answer)).ToList()
                            }).OrderBy(question => question.DisplayOrder).ToList()
                        }).ToList()
            };

            ViewBag.HideNext = hideNext;

            return viewModel;
        }

        private async Task<Session> GetSessionForShowingAnswerAsync(Guid id)
        {
            return await sessionRepository.RetrieveAsync(id, new ListOptions<Session>(
                    s => s.SessionAnswers,
                    s => s.SessionQuestions,
                    s => s.QuizSessions[0].Quiz.QuizQuestions[0].Question.Answers));
        }
    }
}
