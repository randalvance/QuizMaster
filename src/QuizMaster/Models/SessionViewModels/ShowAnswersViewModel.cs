using QuizMaster.Models.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.SessionViewModels
{
    public class ShowAnswersPageViewModel
    {
        public Guid SessionId { get; set; }
        public List<ShowAnswersQuizViewModel> Quizes { get; set; }
    }

    public class ShowAnswersQuizViewModel
    {
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; }
        public string QuizInstructions { get; set; }
        public bool AnswersOrderImportant { get; set; }

        public List<ShowAnswersQuestionViewModel> Questions { get; set; }
    }

    public class ShowAnswersQuestionViewModel
    {
        public Guid QuestionId { get; set; }
        public int DisplayOrder { get; set; }
        public string QuestionText { get; set; }
        public List<ShowAnswersAnswerViewModel> Answers { get; set; }
    }

    public class ShowAnswersAnswerViewModel
    {
        public Guid AnswerId { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
