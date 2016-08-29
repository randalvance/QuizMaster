using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class SessionQuestion
    {
        public Guid SessionQuestionId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SessionId { get; set; }
        public int DisplayOrder { get; set; }

        public Session Session { get; set; }
        public Question Question { get; set; }
    }
}
