using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public List<SessionAnswer> SessionAnswers { get; set; }
        public int OrderNumber { get; set; }
        public string AnswerText { get; set; }
    }
}
