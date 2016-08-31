using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaker.Data.Repositories
{
    public class QuizGroupRepository : BaseRepository<QuizGroup>
    {
        public QuizGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
