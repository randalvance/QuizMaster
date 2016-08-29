using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class QuizQuestion
    {
        public Guid QuizId { get; set; }
        public Guid QuestionId { get; set; }
        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
    }
}
