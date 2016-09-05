using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Models
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionId { get; set; }
        [Required]
        public string QuestionText { get; set; }

        public List<Answer> Answers { get; set; }
        public List<QuizQuestion> QuizQuestions { get; set; }
        public List<QuestionChoice> Choices { get; set; }
        public List<SessionQuestion> SessionQuestions { get; set; }
    }
}
