using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.Data.Repositories;
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
        private QuizCategoryRepository quizCategoryRepository;

        public QuizCategoryController(QuizCategoryRepository quizCategoryRepository)
        {
            this.quizCategoryRepository = quizCategoryRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new QuizCategoryListViewModel()
            {
                Categories = quizCategoryRepository.List().ToList()
            };

            return View(viewModel);
        }

        public IActionResult Add()
        {
            var viewModel = new QuizCategoryEditViewModel();

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var quizCategory = await quizCategoryRepository.RetrieveAsync(id);

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

        public async Task<IActionResult> Edit(Guid id)
        {
            var quizCategory = await quizCategoryRepository.RetrieveAsync(id);
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

            await quizCategoryRepository.AddAsync(quizCategory);
            await quizCategoryRepository.CommitAsync();

            return RedirectToAction("Index", new { addSuccess = true });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuizCategoryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quizCategory = await quizCategoryRepository.RetrieveAsync(viewModel.QuizCategoryId);
            quizCategory.Name = viewModel.Name;
            quizCategory.Description = viewModel.Description;
            await quizCategoryRepository.UpdateAsync(quizCategory);
            await quizCategoryRepository.CommitAsync();

            return RedirectToAction("Index", new { addSuccess = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var quizCategory = await quizCategoryRepository.RetrieveAsync(id);
            await quizCategoryRepository.RemoveAsync(quizCategory);
            await quizCategoryRepository.CommitAsync();

            return RedirectToAction("Index", new { deleteSuccess = true });
        }
    }
}
