﻿using QuizMaster.Models;
using QuizMaster.Data;

namespace QuizMaster.Data.Repositories
{
    public class QuizCategoryRepository : BaseRepository<QuizCategory>
    {
        public QuizCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
