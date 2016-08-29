using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.SessionViewModels
{
    public class SessionAnswerViewModel
    {
        public Guid AnswerId { get; set; }
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; }
        public string QuizInstructions { get; set; }
        public int Order { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool AnswersOrderImportant { get; set; }
        public bool IsCorrect { get; set; }
    }
}
