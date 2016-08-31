using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaster.Data.Repositories
{
    public class QuizGroupRepository : BaseRepository<QuizGroup>
    {
        public QuizGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
