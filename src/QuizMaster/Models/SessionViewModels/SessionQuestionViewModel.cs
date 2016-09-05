using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace QuizMaster.Models.SessionViewModels
{
    public class SessionQuestionViewModel
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int DisplayOrder { get; set; }
        public List<SessionAnswerViewModel> Answers { get; set; }
        public SelectList Choices { get; set; }
    }
}
