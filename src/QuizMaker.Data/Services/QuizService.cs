﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuizMaker.Data.Services
{
    public class QuizService : ServiceBase
    {
        public QuizService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext, userManager)
        {
        }

        public async Task<Guid> GetRecommendedQuizAsync(Guid userId)
        {
            return await ExecuteStoredProcedureScalarResultAsync<Guid>(
                "GetRecommendedQuizId",
                new List<SqlParameter> { new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId } });
        }

        public async Task<int> GetQuizOfTheDaySequenceNumberAsync(ClaimsPrincipal user)
        {
            var usr = await UserManager.FindByNameAsync(user.Identity.Name);
            return await DbContext.Sessions.Where(x => x.ApplicationUserId == usr.Id && x.DateCompleted.HasValue && x.DateCompleted.Value.Date == DateTime.Now.Date).CountAsync() + 1;
        }
    }
}
