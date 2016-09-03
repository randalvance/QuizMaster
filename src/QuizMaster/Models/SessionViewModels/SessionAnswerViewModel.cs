using System;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Models.SessionViewModels
{
    public class SessionAnswerViewModel
    {
        public Guid AnswerId { get; set; }
        public int Order { get; set; }
        public string CorrectAnswer { get; set; }
        [Required(ErrorMessage = "Answer is required!")]
        public string UserAnswer { get; set; }
        public bool AnswersOrderImportant { get; set; }
        public bool IsCorrect { get; set; }
    }
}
