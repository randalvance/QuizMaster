using Microsoft.AspNetCore.Mvc;

namespace QuizMaker.ViewComponents
{
    public class ReferenceViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string myCustomProp)
        {
            var viewModel = new ReferenceViewComponentViewModel
            {
                Value = myCustomProp
            };

            // Will use view named Views/Shared/Components/Reference/Default.cshtml
            // In a consuming view, use @Component.InvokeAsync("Reference", new { myCustomProp = "Foo" })
            return View(viewModel);
        }
    }

    public class ReferenceViewComponentViewModel
    {
        public string Value { get; set; }
    }
}
