using QuizMaker.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizViewModels
{
    public class ShowAnswersViewModel
    {
        public Guid SessionId { get; set; }
        public List<ShowAnswersQuizGroupViewModel> QuizGroups { get; set; }
        public bool HideNext { get; set; }
    }
}
