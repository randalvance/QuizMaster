using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Controllers.BaseControllers;
using QuizMaster.Data.Constants;
using QuizMaster.Data.Core;
using QuizMaster.Data.Repositories;
using QuizMaster.Models;
using QuizMaster.Models.QuizViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    [Authorize(Roles = IdentityConstants.SuperAdministratorRoleName)]
    public class QuizGroupController : ToastController
    {
        private QuizGroupRepository quizGroupRepository;
        private QuizCategoryRepository quizCategoryRepository;

        public QuizGroupController(
            QuizCategoryRepository quizCategoryRepository,
            QuizGroupRepository quizGroupRepository)
        {
            this.quizCategoryRepository = quizCategoryRepository;
            this.quizGroupRepository = quizGroupRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new QuizGroupListViewModel()
            {
                Groups = quizGroupRepository.RetrieveAll(new ListOptions<QuizGroup>(x => x.QuizCategory)).ToList()
            };

            EmbedToastOptions();

            return View(viewModel);
        }

        public IActionResult Add()
        {
            var viewModel = new QuizGroupEditViewModel()
            {
                Categories = quizCategoryRepository.RetrieveAll().ToList()
            };

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var quizGroup = await quizGroupRepository.RetrieveAsync(id,
                new ListOptions<QuizGroup>(x => x.Quizes, x => x.QuizCategory));

            var viewModel = new QuizGroupEditViewModel()
            {
                QuizGroupId = id,
                Code = quizGroup.Code,
                Name = quizGroup.Name,
                Description = quizGroup.Description,
                Categories = quizCategoryRepository.RetrieveAll().ToList(),
                Quizes = quizGroup.Quizes.ToList(),
                QuizCategoryName = quizGroup.QuizCategory.Name,
                ReadOnly = true
            };

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var quizGroup = await quizGroupRepository.RetrieveAsync(id);

            var viewModel = new QuizGroupEditViewModel()
            {
                QuizGroupId = id,
                Code = quizGroup.Code,
                Name = quizGroup.Name,
                Description = quizGroup.Description,
                Categories = quizCategoryRepository.RetrieveAll().ToList(),
                QuizCategoryId = quizGroup.QuizCategoryId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(QuizGroupEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quizGroup = new QuizGroup()
            {
                Code = viewModel.Code,
                Name = viewModel.Name,
                Description = viewModel.Description,
                QuizCategoryId = viewModel.QuizCategoryId
            };

            await quizGroupRepository.AddAsync(quizGroup);
            await quizGroupRepository.CommitAsync();

            ToastSuccess($"{quizGroup.Name} has been added.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuizGroupEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quizGroup = await quizGroupRepository.RetrieveAsync(viewModel.QuizGroupId);

            quizGroup.Name = viewModel.Name;
            quizGroup.Description = viewModel.Description;
            quizGroup.QuizCategoryId = viewModel.QuizCategoryId;

            await quizGroupRepository.UpdateAsync(quizGroup);
            await quizGroupRepository.CommitAsync();

            ToastSuccess($"{quizGroup.Name} has been updated.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var quizGroup = await quizGroupRepository.RetrieveAsync(id);
            await quizGroupRepository.RemoveAsync(quizGroup);
            await quizGroupRepository.CommitAsync();

            ToastSuccess($"{quizGroup.Name} has been deleted.");

            return RedirectToAction("Index");
        }
    }
}
