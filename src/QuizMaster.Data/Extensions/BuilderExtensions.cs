using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Data;
using QuizMaster.Data.Extensions;
using QuizMaster.Models;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static void UseAppDatabase(this IApplicationBuilder app, string initialQuizFolder, ApplicationDbContext dbContext, 
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            QuizMaster.Options.IdentityOptions identityOptions)
        {
            dbContext.Database.Migrate();
            app.SeedDataAsync(initialQuizFolder, dbContext, userManager, roleManager, identityOptions);
        }
    }
}
