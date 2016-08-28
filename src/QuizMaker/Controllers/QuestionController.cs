using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.Data;
using QuizMaker.Models.QuestionViewModels;
using QuizMaker.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Constants;

namespace QuizMaker.Controllers
{
    [Authorize(Roles = IdentityConstants.SuperAdministratorRoleName)]
    public class QuestionController : Controller
    {
        private ApplicationDbContext appDbContext;

        public QuestionController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new QuestionManagePageViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(QuestionManagePageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var quiz = QuestionParser.ConvertTextToQuiz(viewModel.Content);

            var quizGroup = appDbContext.QuizGroups.SingleOrDefault(q => q.Code == quiz.QuizGroupCode);
            quiz.QuizGroupId = quizGroup.QuizGroupId;

            appDbContext.Quizes.Add(quiz);
            appDbContext.SaveChanges();

            return View(new QuestionManagePageViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var quizes = await appDbContext.Quizes.Include(q => q.QuizSessions).Include(q => q.QuizQuestions).ThenInclude(q => q.Question).ToListAsync();

            return View(quizes);
        }
    }
}
