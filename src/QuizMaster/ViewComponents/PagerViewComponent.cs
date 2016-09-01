using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
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
