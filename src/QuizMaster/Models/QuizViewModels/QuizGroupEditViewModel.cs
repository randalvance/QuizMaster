using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.QuizViewModels
{
    public class QuizGroupEditViewModel
    {

        public Guid QuizGroupId { get; set; }
        [Required]
        public Guid QuizCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string Description { get; set; }

        public string QuizCategoryName { get; set; }

        public List<QuizCategory> Categories { get; set; }

        public List<Quiz> Quizes { get; set; }

        public string ArticleContentHtml { get; set; }

        public bool ReadOnly { get; set; }
    }
}
