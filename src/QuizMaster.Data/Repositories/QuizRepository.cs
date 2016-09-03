using QuizMaster.Common;
using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaster.Data.Repositories
{
    public class QuizRepository : BaseRepository<Quiz>
    {
        public QuizRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }
    }
}
