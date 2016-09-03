using QuizMaster.Common;
using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaster.Data.Repositories
{
    public class QuizGroupRepository : BaseRepository<QuizGroup>
    {
        public QuizGroupRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }
    }
}
