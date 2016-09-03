using QuizMaster.Common;
using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaster.Data.Repositories
{
    public class SessionAnswerRepository : BaseRepository<SessionAnswer>
    {
        public SessionAnswerRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }
    }
}
