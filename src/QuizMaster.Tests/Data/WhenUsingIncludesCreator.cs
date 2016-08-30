using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Data.Core;
using QuizMaster.Data;
using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuizMaster.Tests.Data
{
    public class WhenUsingIncludesCreator
    {
        private IncludesCreator<Session> sessionIncludesCreator;

        public WhenUsingIncludesCreator()
        {
            sessionIncludesCreator = new IncludesCreator<Session>();            
        }

        [Fact]
        public void ShouldProperlySplitPropertiesAndReturnTheCorrectCountOfFuncs()
        {
            var resultsWithCollection1 = sessionIncludesCreator.GetFuncsFromExpression(s => s.QuizSessions);
            var resultsWithCollection2 = sessionIncludesCreator.GetFuncsFromExpression(s => s.QuizSessions[0].Quiz);
            var resultsWithCollection3 = sessionIncludesCreator.GetFuncsFromExpression(s => s.SessionQuestions[0].Question.Answers[0].Question);

            var results1 = sessionIncludesCreator.GetFuncsFromExpression(s => s.ApplicationUser);
            var results2 = sessionIncludesCreator.GetFuncsFromExpression(s => s.ApplicationUser.Logins);
            var results3 = sessionIncludesCreator.GetFuncsFromExpression(s => s.ApplicationUser.SecurityStamp.Length);

            Assert.Equal(1, results1.Count());
            Assert.Equal(2, results2.Count());
            Assert.Equal(3, results3.Count());

            Assert.Equal(1, resultsWithCollection1.Count());
            Assert.Equal(2, resultsWithCollection2.Count());
            Assert.Equal(4, resultsWithCollection3.Count());

            var dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());

            sessionIncludesCreator.ApplyIncludes(new List<Session>().AsQueryable(), x => x.ApplicationUser);
        }

        [Fact]
        public void ShouldIncludeNavigationProperty()
        {
            var serviceProvider = GetServiceProvider();
            var dbContext = serviceProvider.GetService<ApplicationDbContext>();

            var session = sessionIncludesCreator.ApplyIncludes(dbContext.Sessions, 
                s => s.QuizSessions[0].Quiz,
                s => s.SessionAnswers[0].Answer,
                s => s.SessionQuestions,
                s => s.ApplicationUser).FirstOrDefault(s => s.SessionStatus == SessionStatus.Done);

            Assert.NotEmpty(session.QuizSessions);
            Assert.All(session.QuizSessions, s => Assert.NotNull(s.Quiz));

            Assert.NotEmpty(session.SessionAnswers);
            Assert.All(session.SessionAnswers, s => Assert.NotNull(s.Answer));

            Assert.NotEmpty(session.SessionQuestions);
            Assert.NotNull(session.ApplicationUser);
        }

        private IServiceProvider GetServiceProvider()
        {
            var host = new WebHostBuilder();
            var env = host.GetSetting("environment");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services.BuildServiceProvider();
        }
    }
}
