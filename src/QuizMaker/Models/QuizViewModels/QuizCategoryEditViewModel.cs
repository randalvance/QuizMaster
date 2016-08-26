using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizViewModels
{
    public class QuizCategoryEditViewModel
    {
        public Guid QuizCategoryId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ReadOnly { get; set; }
    }
}
