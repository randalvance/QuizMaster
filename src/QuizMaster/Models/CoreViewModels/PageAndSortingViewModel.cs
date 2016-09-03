namespace QuizMaster.Models.CoreViewModels
{
    public class PageAndSortingViewModel
    {
        public PageAndSortingViewModel()
        {
            Page = 1;
            ItemsPerPage = 10;
        }

        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
