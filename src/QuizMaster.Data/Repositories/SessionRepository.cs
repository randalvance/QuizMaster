using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizMaster.Data;

namespace QuizMaker.Data.Repositories
{
    public class SessionRepository : BaseRepository<Session>
    {
        public SessionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
