using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizMaster.Data;
using QuizMaster.Common;

namespace QuizMaster.Data.Repositories
{
    public class SessionRepository : BaseRepository<Session>
    {
        public SessionRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }
    }
}
