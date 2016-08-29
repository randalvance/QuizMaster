using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.SessionViewModels
{
    public class SessionQuestionViewModel
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int DisplayOrder { get; set; }
        public List<SessionAnswerViewModel> Answers { get; set; }
    }
}
