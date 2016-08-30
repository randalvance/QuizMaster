using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Core;
using QuizMaker.Data.Repositories;
using QuizMaster.Data;
using QuizMaster.Data.Constants;
using QuizMaster.Data.Services;
using QuizMaster.Data.Settings;
using QuizMaster.Models;
using QuizMaster.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private ApplicationSettingService appSettingsService;
        private QuizService quizService;
        private QuizSettings quizSettings;
        private SessionRepository sessionRepository;
        private SessionSettings sessionsSettings;

        public SessionController(ApplicationSettingService appSettingsService,
                                 QuizService quizService,
                                 QuizSettings quizSettings,
                                 SessionRepository sessionRepository,
                                 SessionSettings sessionsSettings)
        {
            this.appSettingsService = appSettingsService;
            this.quizService = quizService;
            this.quizSettings = quizSettings;
            this.sessionsSettings = sessionsSettings;
            this.sessionRepository = sessionRepository;
        }

        public async Task<IActionResult> Index(Guid? userId = null)
        {
            var isAdmin = User.IsInRole(IdentityConstants.SuperAdministratorRoleName);

            if (userId == null && !isAdmin)
            {
                throw new UnauthorizedAccessException("You are not an administrator.");
            }

            var sessions = await sessionRepository.List(new ListOptions<Session>(
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
                RequiredQuizes = await sessionsSettings.RecommendedSessionCountPerDay
            };
            return View(viewModel);
        }
    }
}
