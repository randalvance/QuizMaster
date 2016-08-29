using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class QuestionChoice
    {
        public Guid QuestionChoiceId { get; set; }
        public Guid QuestionId { get; set; }
        public int DisplayOrder { get; set; }
        public string Choice { get; set; }

        public Question Question { get; set; }
    }
}
