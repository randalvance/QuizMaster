using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QuizMaster.Data
{
    /// <summary>
    /// This is a workaround to enable migrations on class libraries
    /// http://benjii.me/2016/06/entity-framework-core-migrations-for-class-library-projects/
    /// </summary>
    public class TemporaryDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=pinchdb;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new ApplicationDbContext(builder.Options);
        }
    }
}
