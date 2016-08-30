using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Data;
using QuizMaster.Models;
using QuizMaster.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizMaster.Tests.Data
{
    public class WhenUsingBaseRepository : BaseDataTest
    {
        [Fact]
        public async void ShouldAddANewEntity()
        {
            var options = CreateNewOptions();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var repository = new MockRepository(dbContext);
                var session = new Session();

                await repository.AddAsync(session);
                await repository.CommitAsync();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                Assert.Equal(1, dbContext.Sessions.Count());
            }
        }
    }
}
