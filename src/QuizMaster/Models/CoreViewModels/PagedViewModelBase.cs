using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.CoreViewModels
{
    public class PagedViewModelBase
    {
        public int TotalItems { get; set; }
        public int NumberOfPages
        {
            get
            {
                if (PageAndSorting.ItemsPerPage == 0)
                {
                    return 1;
                }

                int pages = TotalItems / PageAndSorting.ItemsPerPage;
                int extra = TotalItems % PageAndSorting.ItemsPerPage;

                if (extra > 0)
                {
                    pages++;
                }

                return pages;
            }
        }

        public PageAndSortingViewModel PageAndSorting { get; set; } = new PageAndSortingViewModel();
    }
}
