using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Data;
using QuizMaster.Data.Constants;
using QuizMaster.Models;
using QuizMaster.Models.QuizViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    [Authorize(Roles = IdentityConstants.SuperAdministratorRoleName)]
    public class QuizCategoryController : Controller
    {
        private ApplicationDbContext appDbContext;

        public QuizCategoryController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var viewModel = new QuizCategoryListViewModel()
            {
                Categories = appDbContext.QuizCategories.ToList()
            };

            return View(viewModel);
        }

        public IActionResult Add()
        {
            var viewModel = new QuizCategoryEditViewModel();

            return View("Edit", viewModel);
        }

        public IActionResult Detail(Guid id)
        {
            var quizCategory = appDbContext.QuizCategories.SingleOrDefault(x => x.QuizCategoryId == id);
            var viewModel = new QuizCategoryEditViewModel()
            {
                QuizCategoryId = id,
                Code = quizCategory.Code,
                Name = quizCategory.Name,
                Description = quizCategory.Description,
                ReadOnly = true
            };

            return View("Edit", viewModel);
        }

        public IActionResult Edit(Guid id)
        {
            var quizCategory = appDbContext.QuizCategories.SingleOrDefault(x => x.QuizCategoryId == id);
            var viewModel = new QuizCategoryEditViewModel()
            {
                QuizCategoryId = id,
                Code = quizCategory.Code,
                Name = quizCategory.Name,
                Description = quizCategory.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(QuizCategoryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quizCategory = new QuizCategory()
            {
                Code = viewModel.Code,
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            appDbContext.QuizCategories.Add(quizCategory);
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { addSuccess = true });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuizCategoryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quizCategory = appDbContext.QuizCategories.SingleOrDefault(x => x.QuizCategoryId == viewModel.QuizCategoryId);
            quizCategory.Code = viewModel.Code;
            quizCategory.Name = viewModel.Name;
            quizCategory.Description = viewModel.Description;
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { addSuccess = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var quizCategory = appDbContext.QuizCategories.SingleOrDefault(x => x.QuizCategoryId == categoryId);
            appDbContext.Remove(quizCategory);
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { deleteSuccess = true });
        }
    }
}
