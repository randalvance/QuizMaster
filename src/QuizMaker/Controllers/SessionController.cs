using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
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

        public SessionController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new SessionListViewModel()
            {
                Sessions = await appDbContext.Sessions
                            .Include(s => s.ApplicationUser)
                            .Include(s => s.QuizSessions).ThenInclude(qs => qs.Quiz)
                            .Where(s => (s.DateCompleted.HasValue ? s.DateCompleted.Value : s.DateTaken).Date == DateTime.Now.Date)
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
                            }).ToListAsync()
            };
            return View(viewModel);
        }
    }
}
