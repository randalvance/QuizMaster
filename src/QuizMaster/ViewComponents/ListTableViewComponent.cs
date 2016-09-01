using Microsoft.AspNetCore.Mvc;
using QuizMaster.Models.ComponentViewModels;

namespace QuizMaster.ViewComponents
{
    public class ListTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ListTableComponentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
