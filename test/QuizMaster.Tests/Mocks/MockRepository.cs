using QuizMaker.Data.Repositories;
using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizMaster.Data;

namespace QuizMaster.Tests.Mocks
{
    public class MockRepository : BaseRepository<Session>
    {
        public MockRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
