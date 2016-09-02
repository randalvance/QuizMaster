using QuizMaster.Models;
using QuizMaster.Data;
using QuizMaster.Common;

namespace QuizMaster.Data.Repositories
{
    public class QuizCategoryRepository : BaseRepository<QuizCategory>
    {
        public QuizCategoryRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }
    }
}
