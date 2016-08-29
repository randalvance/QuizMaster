using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class QuizGroupController : Controller
    {
        private ApplicationDbContext appDbContext;

        public QuizGroupController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var viewModel = new QuizGroupListViewModel()
            {
                Groups = appDbContext.QuizGroups.Include(x => x.QuizCategory).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Add()
        {
            var viewModel = new QuizGroupEditViewModel()
            {
                Categories = appDbContext.QuizCategories.ToList()
            };

            return View("Edit", viewModel);
        }

        public IActionResult Detail(Guid id)
        {
            var quizGroup = appDbContext.QuizGroups.Include(x => x.QuizCategory)
                            .Include(x => x.Quizes)
                            .SingleOrDefault(x => x.QuizGroupId == id);
            var viewModel = new QuizGroupEditViewModel()
            {
                QuizGroupId = id,
                Code = quizGroup.Code,
                Name = quizGroup.Name,
                Description = quizGroup.Description,
                Categories = appDbContext.QuizCategories.ToList(),
                Quizes = quizGroup.Quizes.ToList(),
                QuizCategoryName = quizGroup.QuizCategory.Name,
                ReadOnly = true
            };

            return View("Edit", viewModel);
        }

        public IActionResult Edit(Guid id)
        {
            var quizGroup = appDbContext.QuizGroups.SingleOrDefault(x => x.QuizGroupId == id);
            var viewModel = new QuizGroupEditViewModel()
            {
                QuizGroupId = id,
                Code = quizGroup.Code,
                Name = quizGroup.Name,
                Description = quizGroup.Description,
                Categories = appDbContext.QuizCategories.ToList(),
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

            appDbContext.QuizGroups.Add(quizGroup);
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { addSuccess = true });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuizGroupEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var quizGroup = appDbContext.QuizGroups.SingleOrDefault(x => x.QuizGroupId == viewModel.QuizGroupId);
            quizGroup.Code = viewModel.Code;
            quizGroup.Name = viewModel.Name;
            quizGroup.Description = viewModel.Description;
            quizGroup.QuizCategoryId = viewModel.QuizCategoryId;
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { addSuccess = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid groupId)
        {
            var QuizGroup = appDbContext.QuizGroups.SingleOrDefault(x => x.QuizGroupId == groupId);
            appDbContext.Remove(QuizGroup);
            await appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { deleteSuccess = true });
        }
    }
}
