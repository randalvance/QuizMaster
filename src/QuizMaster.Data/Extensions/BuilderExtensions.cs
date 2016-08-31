using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizMaster.Data;
using QuizMaster.Data.Extensions;
using QuizMaster.Models;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static void UseAppDatabase(this IApplicationBuilder app, string initialQuizFolder, IServiceProvider serviceProvider)
        {
            var dbContext = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            var userManager = (UserManager<ApplicationUser>)serviceProvider.GetService(typeof(UserManager<ApplicationUser>));
            var roleManager = (RoleManager<ApplicationRole>)serviceProvider.GetService(typeof(RoleManager<ApplicationRole>));
            var identityOptions = (IOptions<QuizMaster.Options.IdentityOptions>)serviceProvider.GetService(typeof(IOptions<QuizMaster.Options.IdentityOptions>));

            dbContext.Database.Migrate();

            app.SeedDataAsync(initialQuizFolder, dbContext, userManager, roleManager, identityOptions.Value);
        }
    }
}
