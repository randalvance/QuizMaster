using QuizMaster.Common.Models;
using QuizMaster.Data.Core;

namespace QuizMaster.Models.CoreViewModels
{
    public class PagedViewModelBase
    {
        public int TotalItems { get; set; }
        public int NumberOfPages
        {
            get
            {
                if (PagingAndSorting.ItemsPerPage == 0)
                {
                    return 1;
                }

                int pages = TotalItems / PagingAndSorting.ItemsPerPage;
                int extra = TotalItems % PagingAndSorting.ItemsPerPage;

                if (extra > 0)
                {
                    pages++;
                }

                return pages;
            }
        }

        public PagingAndSortingOptions PagingAndSorting { get; set; } = new PagingAndSortingOptions();
    }
}
