using QuizMaster.Common;
using QuizMaster.Models;

namespace QuizMaster.Data.Repositories
{
    public class QuestionChoiceRepository : BaseRepository<QuestionChoice>
    {
        public QuestionChoiceRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }
    }
}
