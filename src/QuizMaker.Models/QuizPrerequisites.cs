using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models
{
    public class QuizPrerequisites
    {
        public Guid QuizId { get; set; }
        public string PrerequisiteQuizCode { get; set; }
    }
}
