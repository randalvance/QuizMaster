using QuizMaster.Models.CoreViewModels;
using System.Collections.Generic;

namespace QuizMaster.Models.QuizViewModels
{
    public class QuizListViewModel : PagedViewModelBase
    {
        public List<Quiz> Quizes { get; set; }
    }
}
