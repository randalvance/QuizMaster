using QuizMaster.Common;
using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuizMaster.Tests.Common
{
    public class WhenUsingSortManager
    {
        [Fact]
        public void ShouldSortAscendingOneProperty()
        {
            var sortManager = new SortManager();
            var quizesToSort = GetQuizes();
            var sortExpression = "Code-ASC";

            var sortedQuizes = sortManager.ApplySorting(sortExpression, quizesToSort);

            Assert.Equal("AAA", sortedQuizes.ElementAt(0).Code);
            Assert.Equal("BBB", sortedQuizes.ElementAt(1).Code);
        }

        [Fact]
        public void ShouldSortDescendingOneProperty()
        {
            var sortManager = new SortManager();
            var quizesToSort = GetQuizes();
            var sortExpression = "Code-DESC";

            var sortedQuizes = sortManager.ApplySorting(sortExpression, quizesToSort).ToList();

            Assert.Equal("AAA", sortedQuizes.ElementAt(sortedQuizes.Count - 1).Code);
            Assert.Equal("BBB", sortedQuizes.ElementAt(sortedQuizes.Count - 2).Code);
        }

        [Fact]
        public void ShouldSortAscendingMultipleProperties()
        {
            var sortManager = new SortManager();
            var quizesToSort = GetQuizes();
            var sortExpression = "Code-ASC,Title-ASC";

            var sortedQuizes = sortManager.ApplySorting(sortExpression, quizesToSort).ToList();

            Assert.Equal("CTitle3", sortedQuizes.ElementAt(sortedQuizes.Count - 1).Title);
            Assert.Equal("CTitle2", sortedQuizes.ElementAt(sortedQuizes.Count - 2).Title);
            Assert.Equal("CTitle1", sortedQuizes.ElementAt(sortedQuizes.Count - 3).Title);
        }

        [Fact]
        public void ShouldSortDescendingMultipleProperties()
        {
            var sortManager = new SortManager();
            var quizesToSort = GetQuizes();
            var sortExpression = "Code-DESC,Title-DESC";

            var sortedQuizes = sortManager.ApplySorting(sortExpression, quizesToSort).ToList();

            Assert.Equal("CTitle3", sortedQuizes.ElementAt(0).Title);
            Assert.Equal("CTitle2", sortedQuizes.ElementAt(1).Title);
            Assert.Equal("CTitle1", sortedQuizes.ElementAt(2).Title);
        }

        [Fact]
        public void ShouldSortAscendingDescendingMultipleProperties()
        {
            var sortManager = new SortManager();
            var quizesToSort = GetQuizes();
            var sortExpression = "Code-ASC,Title-DESC";

            var sortedQuizes = sortManager.ApplySorting(sortExpression, quizesToSort).ToList();

            Assert.Equal("CTitle3", sortedQuizes.ElementAt(sortedQuizes.Count - 3).Title);
            Assert.Equal("CTitle2", sortedQuizes.ElementAt(sortedQuizes.Count - 2).Title);
            Assert.Equal("CTitle1", sortedQuizes.ElementAt(sortedQuizes.Count - 1).Title);
        }

        [Fact]
        public void ShouldThrowExceptionWhenInvalidOrderKeyIsFound()
        {
            var sortManager = new SortManager();
            var quizesToSort = GetQuizes();
            var sortExpression = "Code-ASC,Title-AXC";

            var exception = Assert.Throws<ArgumentException>(() => sortManager.ApplySorting(sortExpression, quizesToSort));

            Assert.Equal("Invalid sort order key AXC found for Title", exception.Message);
        }

        public IQueryable<Quiz> GetQuizes()
        {
            var quizes = new List<Quiz>()
            {
                new Quiz { Code = "BBB", Title = "BTitle", QuizGroup = new QuizGroup() { Code = "4Group" } },
                new Quiz { Code = "CCC", Title = "CTitle3", QuizGroup = new QuizGroup() { Code = "3Group" } },
                new Quiz { Code = "CCC", Title = "CTitle1", QuizGroup = new QuizGroup() { Code = "1Group" } },
                new Quiz { Code = "AAA", Title = "ATitle", QuizGroup = new QuizGroup() { Code = "5Group" }},
                new Quiz { Code = "CCC", Title = "CTitle2", QuizGroup = new QuizGroup() { Code = "2Group" } }
            };

            return quizes.AsQueryable();
        }
    }
}
