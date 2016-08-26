using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models
{
    public class QuizChoice
    {
        public Guid QuizChoiceId { get; set; }
        public Guid QuizId { get; set; }
        public int DisplayOrder { get; set; }
        public string Choice { get; set; }

        public Quiz Quiz { get; set; }
    }
}
