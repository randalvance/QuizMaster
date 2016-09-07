using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.QuizViewModels
{
    public class QuizEditViewModel
    {
        public Guid QuizId { get; set; }
        [Required]
        public Guid QuizGroupId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Instructions { get; set; }
        [Required]
        public QuizType QuizType { get; set; }

        public string QuizChoices { get; set; }
        
        public string QuizGroupName { get; set; }

        public bool ReadOnly { get; set; }

        public string ReturnUrl { get; set; }

        public List<QuizGroup> Groups { get; set; }
        
        public List<QuizEditQuestionViewModel> Questions { get; set; }

        public SelectList Types
        {
            get
            {
                var quizTypes = ((QuizType[])Enum.GetValues(typeof(QuizType))).Select(x => new { Value = x, Name = x.ToString() });
                return new SelectList(quizTypes, "Value", "Name", null);
            }
        }
    }

    public class QuizEditQuestionViewModel
    {
        public Guid QuestionId { get; set; }
        [Required]
        public string QuestionText { get; set; }
        /// <summary>
        /// Colon separated answers
        /// </summary>
        [Required]
        public string AnswerData { get; set; }
        // Colon separated choices
        public string ChoicesData { get; set; }
    }
}
