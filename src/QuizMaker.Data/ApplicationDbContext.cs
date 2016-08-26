using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Models;
using System;

namespace QuizMaker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Quiz>().HasAlternateKey(x => x.Code).HasName("UQ_Quiz_Code");
            builder.Entity<QuizCategory>().HasAlternateKey(x => x.Code).HasName("UQ_QuizCategory_Code");
            builder.Entity<QuizGroup>().HasAlternateKey(x => x.Code).HasName("UQ_QuizGroup_Code");
            builder.Entity<QuizSession>().HasKey(x => new { x.QuizId, x.SessionId });
            builder.Entity<QuizQuestion>().HasKey(x => new { x.QuizId, x.QuestionId });
            builder.Entity<QuizPrerequisites>().HasKey(x => new { x.QuizId, x.PrerequisiteQuizCode });
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<QuizCategory> QuizCategories { get; set; }
        public DbSet<QuizGroup> QuizGroups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizSession> QuizSessions { get; set;}
        public DbSet<SessionAnswer> SessionAnswers { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizChoice> QuizChoices { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
    }
}
