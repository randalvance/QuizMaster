using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models.CoreViewModels;

namespace QuizMaster.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PagedViewModelBase viewModel)
        {
            return View(viewModel);
        }
    }
}
