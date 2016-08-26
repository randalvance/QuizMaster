using QuizMaker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Common
{
    public static class QuestionParser
    {
        public static List<Quiz> ParseQuizFiles(string initialQuizFolder)
        {
            var quizes = new List<Quiz>();
            var dir = new DirectoryInfo(initialQuizFolder);
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                Quiz quiz = ConvertFileToQuiz(file);

                if (quiz != null) quizes.Add(quiz);
            }

            return quizes;
        }

        public static Quiz ConvertFileToQuiz(FileInfo file)
        {
            var contents = File.ReadAllText(file.FullName);

            return ConvertTextToQuiz(contents);
        }

        public static Quiz ConvertTextToQuiz(string contents)
        {
            var lines = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Validate(lines);

            var quiz = new Quiz();
            quiz.QuizQuestions = new List<QuizQuestion>();
            quiz.Title = lines[0].Replace("Title:", "").Trim();
            quiz.Code = lines[1].Replace("Code:", "").Trim();
            quiz.QuizGroupCode = lines[2].Replace("QuizGroup:", "").Trim();
            quiz.Instructions = lines[3].Replace("Instructions:", "").Trim();
            quiz.QuizType = GetQuizType(lines[4]);
            quiz.QuizPrerequisites = GetQuizPrerequisites(lines[5]);

            for (int i = 7; i < lines.Length; i++)
            {
                var line = lines[i];
                var question = new Question();
                var tokens = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                // skip questions with no answers
                if (tokens.Length < 2) continue;

                question.QuestionText = tokens[0];
                question.Answers = new List<Answer>();

                for (int j = 1; j < tokens.Length; j++)
                {
                    var answer = new Answer();
                    answer.OrderNumber = j;
                    answer.AnswerText = tokens[j];
                    question.Answers.Add(answer);
                }

                var quizQuestion = new QuizQuestion()
                {
                    Quiz = quiz,
                    Question = question
                };

                quiz.QuizQuestions.Add(quizQuestion);
            }

            return quiz;
        }

        private static void Validate(string[] lines)
        {
            if (!lines[0].StartsWith("Title"))
            {
                throw new InvalidOperationException("Expecting a title in the quiz file.");
            }
            if (!lines[1].StartsWith("Code"))
            {
                throw new InvalidOperationException("Expecting a code in the quiz file.");
            }
            if (!lines[2].StartsWith("QuizGroup"))
            {
                throw new InvalidOperationException("Expecting a code in the quiz file.");
            }
            if (!lines[3].StartsWith("Instructions"))
            {
                throw new InvalidOperationException("Expecting instructions in the quiz file.");
            }
            if (!lines[4].StartsWith("QuizType"))
            {
                throw new InvalidOperationException("Expecting quiz type in the quiz file.");
            }
            if (!lines[5].StartsWith("Prerequisites"))
            {
                throw new InvalidOperationException("Expecting prerequisites in the quiz file.");
            }
            if (!lines[6].StartsWith("Questions"))
            {
                throw new InvalidOperationException("Expecting questions in the quiz file.");
            }
        }

        private static QuizType GetQuizType(string line)
        {
            QuizType quizType = QuizType.Plain;
            line = line.Replace("QuizType:", "").Trim();

            int quizTypeEnumValue;

            if (int.TryParse(line, out quizTypeEnumValue))
            {
                quizType = (QuizType)Enum.ToObject(typeof(QuizType), quizTypeEnumValue);
            }

            return quizType;
        }

        private static List<QuizPrerequisites> GetQuizPrerequisites(string line)
        {
            line = line.Replace("Prerequisites:", "").Trim();
            var codes = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var prerequisites = codes.Select(c => new QuizPrerequisites() { PrerequisiteQuizCode = c });

            return prerequisites.ToList();
        }
    }
}
