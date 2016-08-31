using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaker.Data.Repositories
{
    public class QuizRepository : BaseRepository<Quiz>
    {
        public QuizRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
