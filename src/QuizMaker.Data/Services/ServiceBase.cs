using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Data.Services
{
    public abstract class ServiceBase
    {
        public ServiceBase(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.DbContext = dbContext;
            this.UserManager = userManager;
        }
        
        protected ApplicationDbContext DbContext { get; set; }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected async Task<T> ExecuteStoredProcedureScalarResultAsync<T>(string commandText, IEnumerable<SqlParameter> parameters)
        {
            T result = default(T);

            using (var cmd = DbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters.ToArray());

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        result = (T)dataReader.GetValue(0);
                    }
                }
            }

            return result;
        }
    }
}
