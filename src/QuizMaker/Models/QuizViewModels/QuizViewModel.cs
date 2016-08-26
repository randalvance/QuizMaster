using Microsoft.AspNetCore.Mvc.Rendering;
using QuizMaker.Models.SessionViewModels;
using System;
using System.Collections.Generic;

namespace QuizMaker.Models.QuizViewModels
{
    public class QuizViewModel
    {
        public QuizViewModel()
        {
            Questions = new List<SessionQuestionViewModel>();
            IncorrectAnswers = new List<Guid>();
        }
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int QuizItemCount { get; set; }
        public List<SessionQuestionViewModel> Questions { get; set; }
        public List<Guid> IncorrectAnswers { get; set; }
        public SelectList QuizChoices { get; set; }
        public bool AnswersOrderImportant { get; set; }
    }
}
