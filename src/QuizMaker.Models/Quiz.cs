using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models
{
    public class Quiz
    {
        public Quiz()
        {
            CreateDate = DateTime.Now;
            ModifyDate = DateTime.Now;

            QuizQuestions = new List<QuizQuestion>();
            QuizSessions = new List<QuizSession>();
            QuizPrerequisites = new List<QuizPrerequisites>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuizId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        public string Instructions { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
        [Required]
        public Guid QuizGroupId { get; set; }
        [Required]
        public QuizType QuizType { get; set; }

        public bool AnswersOrderImportant { get; set; }

        public QuizGroup QuizGroup { get; set; }
        public List<QuizSession> QuizSessions { get; set; }
        public List<QuizQuestion> QuizQuestions { get; set; }
        public List<QuizChoice> QuizChoices { get; set; }
        public List<QuizPrerequisites> QuizPrerequisites { get; set; }

        // Temporary will remove later
        public string QuizGroupCode { get; set; }
    }
}
