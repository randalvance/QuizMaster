using QuizMaster.Data;
using QuizMaster.Models;

namespace QuizMaster.Data.Repositories
{
    public class SessionAnswerRepository : BaseRepository<SessionAnswer>
    {
        public SessionAnswerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
