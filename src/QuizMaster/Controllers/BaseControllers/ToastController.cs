using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizMaster.Models.CoreViewModels;

namespace QuizMaster.Controllers.BaseControllers
{
    public abstract class ToastController : Controller
    {
        protected void ToastSuccess(string message)
        {
            TempData["ToastOptions"] = JsonConvert.SerializeObject(new ToastViewModel()
            {
                ToastType = ToastType.Success,
                Title = "Success",
                Message = message
            });
        }

        protected void EmbedToastOptions()
        {
            ViewBag.ToastOptions = TempData["ToastOptions"];
        }
    }
}
