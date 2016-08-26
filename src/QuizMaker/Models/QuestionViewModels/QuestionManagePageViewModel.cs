using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionViewModels
{
    public class QuestionManagePageViewModel
    {
        [Required]
        public string Content { get; set; }
    }
}
