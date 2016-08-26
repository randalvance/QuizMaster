using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models.SessionViewModels
{
    public class SessionViewModel
    {
        public Guid SessionId { get; set; }
        public string UserName { get; set; }
        public string SessionStatus { get; set; }
        public string QuizTitle { get; set; }
        public string QuizDescription { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int QuizItemCount { get; set; }
    }
}
