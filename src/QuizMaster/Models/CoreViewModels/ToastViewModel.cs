namespace QuizMaster.Models.CoreViewModels
{
    public class ToastViewModel
    {
        public ToastType ToastType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool EscapeHtml { get; set; }
    }

    public enum ToastType
    {
        Success,
        Warning,
        Error
    }
}
