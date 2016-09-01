using QuizMaster.Common;
using QuizMaster.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuizMaster.Tests.Common
{
    public class WhenUsingExpressionParser
    {
        [Fact]
        public void ShouldProperlySplitPropertiesAndReturnTheCorrectCountOfFuncs()
        {
            var resultsWithCollection1 = ExpressionParser.GetFuncsFromExpression<Session>(s => s.QuizSessions);
            var resultsWithCollection2 = ExpressionParser.GetFuncsFromExpression<Session>(s => s.QuizSessions[0].Quiz);
            var resultsWithCollection3 = ExpressionParser.GetFuncsFromExpression<Session>(s => s.SessionQuestions[0].Question.Answers[0].Question);

            var results1 = ExpressionParser.GetFuncsFromExpression<Session>(s => s.ApplicationUser);
            var results2 = ExpressionParser.GetFuncsFromExpression<Session>(s => s.ApplicationUser.Logins);
            var results3 = ExpressionParser.GetFuncsFromExpression<Session>(s => s.ApplicationUser.SecurityStamp.Length);

            Assert.Equal(1, results1.Count());
            Assert.Equal(2, results2.Count());
            Assert.Equal(3, results3.Count());

            Assert.Equal(1, resultsWithCollection1.Count());
            Assert.Equal(2, resultsWithCollection2.Count());
            Assert.Equal(4, resultsWithCollection3.Count());
        }

        [Fact]
        public void ShouldGetPropertyFromPath()
        {
            var quiz = new Quiz();
            quiz.Code = "Test";
            quiz.QuizGroup = new QuizGroup()
            {
                Code = "TestGroup",
                QuizCategory = new QuizCategory()
                {
                    Code = "TestCategory"
                }
            };
            quiz.QuizQuestions = new List<QuizQuestion>()
            {
                new QuizQuestion()
                {
                    Question = new Question { QuestionText = "Question1" }
                },
                new QuizQuestion()
                {
                    Question = new Question { QuestionText = "Question2" }
                }
            };

            var quizCode = ExpressionParser.GetValueFromProperty(quiz, x => x.Code);
            var groupCode = ExpressionParser.GetValueFromProperty(quiz, x => x.QuizGroup.Code);
            var categoryCode = ExpressionParser.GetValueFromProperty(quiz, x => x.QuizGroup.QuizCategory.Code);
            var firstQuestion = ExpressionParser.GetValueFromProperty(quiz, x => x.QuizQuestions[0].Question.QuestionText);
            var secondQuestion = ExpressionParser.GetValueFromProperty(quiz, x => x.QuizQuestions[1].Question.QuestionText);

            Assert.Equal("Test", quizCode);
            Assert.Equal("TestGroup", groupCode);
            Assert.Equal("TestCategory", categoryCode);
            Assert.Equal("Question1", firstQuestion);
            Assert.Equal("Question2", secondQuestion);
        }
    }
}
