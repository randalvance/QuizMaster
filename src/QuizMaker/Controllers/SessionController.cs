using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.Data.Constants;
using QuizMaker.Data.Services;
using QuizMaker.Data.Settings;
using QuizMaker.Models.SessionViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private ApplicationDbContext appDbContext;
        private QuizService quizService;
        private ApplicationSettingService appSettingsService;
        private SessionSettings sessionsSettings;
        private QuizSettings quizSettings;

        public SessionController(ApplicationDbContext appDbContext,
                                 ApplicationSettingService appSettingsService,
                                 QuizService quizService,
                                 SessionSettings sessionsSettings,
                                 QuizSettings quizSettings)
        {
            this.appDbContext = appDbContext;
            this.appSettingsService = appSettingsService;
            this.quizService = quizService;
            this.sessionsSettings = sessionsSettings;
            this.quizSettings = quizSettings;
        }

        public async Task<IActionResult> Index(Guid? userId = null)
        {
            var isAdmin = User.IsInRole(IdentityConstants.SuperAdministratorRoleName);

            if (userId == null && !isAdmin)
            {
                throw new UnauthorizedAccessException("You are not an administrator.");
            }

            var sessions = await appDbContext.Sessions
                            .Include(s => s.ApplicationUser)
                            .Include(s => s.QuizSessions).ThenInclude(qs => qs.Quiz)
                            .Where(s => (s.DateCompleted.HasValue ? s.DateCompleted.Value : s.DateTaken).Date == DateTime.Now.Date &&
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
