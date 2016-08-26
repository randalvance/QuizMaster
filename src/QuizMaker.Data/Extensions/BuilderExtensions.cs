using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.Data.Extensions;
using QuizMaker.Models;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static void UseAppDatabase(this IApplicationBuilder app, string initialQuizFolder, ApplicationDbContext dbContext, 
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            QuizMaker.Options.IdentityOptions identityOptions)
        {
            dbContext.Database.Migrate();
            app.SeedDataAsync(initialQuizFolder, dbContext, userManager, roleManager, identityOptions);
        }
    }
}
