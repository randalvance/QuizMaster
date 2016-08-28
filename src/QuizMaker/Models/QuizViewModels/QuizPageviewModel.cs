using System;
using System.Collections.Generic;

namespace QuizMaker.Models.QuizViewModels
{
    public class QuizPageViewModel
    {
        public QuizPageViewModel()
        {
            Quizes = new List<QuizViewModel>();
        }

        public Guid SessionId { get; set; }
        public bool InitialLoad { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int RetryAnswerCount { get; set; }
        public int QuizItemCount { get; set; }
        public int QuizOfTheDayNumber { get; set; }
        public bool IsRetry { get; set; }
        public List<QuizViewModel> Quizes { get; set; }
    }
}
