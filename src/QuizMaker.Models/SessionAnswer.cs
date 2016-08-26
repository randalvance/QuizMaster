using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models
{
    public class SessionAnswer
    {
        public Guid SessionAnswerId { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }
        public bool IsCorrect { get; set; }
        public string UserAnswer { get; set; }
        public int AnswerChronology { get; set; }
    }
}
