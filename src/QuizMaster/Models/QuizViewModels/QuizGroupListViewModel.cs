using QuizMaster.Models.CoreViewModels;
using System.Collections.Generic;

namespace QuizMaster.Models.QuizViewModels
{
    public class QuizGroupListViewModel : PagedViewModelBase
    {
        public List<QuizGroup> Groups { get; set; }
    }
}
