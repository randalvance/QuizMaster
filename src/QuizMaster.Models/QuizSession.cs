using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class QuizSession
    {
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public Guid SessionId { get; set; }
        public Session Session { get; set; }
    }
}
