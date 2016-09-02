using Moq;
using QuizMaster.Common;
using QuizMaster.Data;
using QuizMaster.Data.Repositories;
using QuizMaster.Models;
using System;
using System.Linq;
using Xunit;

namespace QuizMaster.Tests.Data
{
    public class WhenUsingBaseRepository : BaseDataTest
    {
        [Fact]
        public void ShouldRetrieveAllEntities()
        {
            var options = CreateNewOptions();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Sessions.Add(new Session());
                dbContext.Sessions.Add(new Session());
                dbContext.Sessions.Add(new Session());
                dbContext.SaveChanges();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var sessions = dbContext.Sessions.ToList();
                Assert.Equal(3, sessions.Count);
            }
        }

        [Fact]
        public async void ShouldRetrieveSingleEntityById()
        {
           var mockSortManager = GetSortManager<Session>();

            var options = CreateNewOptions();
            Session sessionToFind;

            using (var dbContext = new ApplicationDbContext(options))
            {
                sessionToFind = new Session();

                dbContext.Sessions.Add(new Session());
                dbContext.Sessions.Add(sessionToFind);
                dbContext.Sessions.Add(new Session());
                dbContext.SaveChanges();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new SessionRepository(dbContext, mockSortManager.Object);
                var foundSession = await repository.RetrieveAsync(sessionToFind.SessionId);

                Assert.NotEqual(foundSession.SessionId, Guid.Empty);
                Assert.Equal(sessionToFind.SessionId, foundSession.SessionId);
            }
        }

        [Fact]
        public async void ShouldAddANewEntity()
        {
            var mockSortManager = GetSortManager<Session>();
            var options = CreateNewOptions();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new SessionRepository(dbContext, mockSortManager.Object);
                var session = new Session();

                await repository.AddAsync(session);
                await repository.CommitAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                Assert.Equal(1, dbContext.Sessions.Count());
            }
        }

        [Fact]
        public async void ShouldRemoveEntity()
        {
            var mockSortManager = GetSortManager<Session>();
            var options = CreateNewOptions();
            Session sessionToFind;

            using (var dbContext = new ApplicationDbContext(options))
            {
                sessionToFind = new Session();

                dbContext.Sessions.Add(new Session());
                dbContext.Sessions.Add(sessionToFind);
                dbContext.Sessions.Add(new Session());
                dbContext.SaveChanges();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new SessionRepository(dbContext, mockSortManager.Object);
                var foundSession = await repository.RetrieveAsync(sessionToFind.SessionId);

                await repository.RemoveAsync(foundSession);
                await repository.CommitAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new SessionRepository(dbContext, mockSortManager.Object);
                var sessions = repository.RetrieveAll().ToList();

                Assert.Equal(2, sessions.Count);
            }
        }

        [Fact]
        public async void ShouldUpdateEntity()
        {
            var mockSortManager = GetSortManager<Quiz>();
            var options = CreateNewOptions();
            Quiz quizToUpdate;

            using (var dbContext = new ApplicationDbContext(options))
            {
                quizToUpdate = new Quiz() { Code = "QUIZ2", Title = "Quiz 2", Instructions = "Instructions 2" };
                dbContext.Quizes.Add(new Quiz() { Code = "QUIZ1", Title = "Quiz 1", Instructions = "Instructions 1" });
                dbContext.Quizes.Add(quizToUpdate);
                dbContext.Quizes.Add(new Quiz() { Code = "QUIZ3", Title = "Quiz 3", Instructions = "Instructions 3" });
                dbContext.SaveChanges();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new QuizRepository(dbContext, mockSortManager.Object);
                quizToUpdate = await repository.RetrieveAsync(quizToUpdate.QuizId);
                quizToUpdate.Title = "Updated Title";
                quizToUpdate.Instructions = "Updated Instructions";
                await repository.UpdateAsync(quizToUpdate);
                await repository.CommitAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new QuizRepository(dbContext, mockSortManager.Object);
                var updatedQuiz = await repository.RetrieveAsync(quizToUpdate.QuizId);

                Assert.Equal("Updated Title", updatedQuiz.Title);
                Assert.Equal("Updated Instructions", updatedQuiz.Instructions);
            }
        }

        [Fact]
        public async void ShouldReturnCorrectCount()
        {
            var mockSortManager = GetSortManager<Session>();
            var options = CreateNewOptions();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Sessions.Add(new Session());
                dbContext.Sessions.Add(new Session());
                dbContext.Sessions.Add(new Session());
                dbContext.SaveChanges();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var sessionRepository = new SessionRepository(dbContext, mockSortManager.Object);
                Assert.Equal(3, await sessionRepository.CountAsync());
            }
        }

        private static Mock<ISortManager> GetSortManager<T>() where T : class
        {
            var mockSortManager = new Mock<ISortManager>();
            mockSortManager.Setup(x => x.ApplySorting<T>(It.IsAny<string>(), It.IsAny<IQueryable<T>>()));
            return mockSortManager;
        }
    }
}
