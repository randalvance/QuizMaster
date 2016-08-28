using QuizMaker.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizViewModels
{
    public class ShowAnswersQuizGroupViewModel
    {
        public string QuizTitle { get; set; }
        public string QuizIndustructions { get; set; }
        public bool AnswersOrderImportant { get; set; }
        public List<SessionAnswerViewModel> Answers { get; set; }
    }
}
