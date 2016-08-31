using Microsoft.AspNetCore.Mvc;
using QuizMaker.Data.Repositories;
using QuizMaster.Models.ApplicationSettingViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    public class ApplicationSettingController : Controller
    {
        private ApplicationSettingRepository applicationSettingRepository;

        public ApplicationSettingController(ApplicationSettingRepository applicationSettingRepository)
        {
            this.applicationSettingRepository = applicationSettingRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ApplicationSettingListPageViewModel()
            {
                ApplicationSettings = (await applicationSettingRepository.RetrievAllAsync()).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApplicationSettingListPageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            foreach(var setting in viewModel.ApplicationSettings)
            {
                await applicationSettingRepository.UpdateAsync(setting);
            }

            await applicationSettingRepository.CommitAsync();

            return View(viewModel);
        }
    }
}
