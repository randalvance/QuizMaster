using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using QuizMaster.Common;
using QuizMaster.Data.Constants;
using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Data.Extensions
{
    public static class DataSeeder
    {
        public async static void SeedDataAsync(this IApplicationBuilder app, 
            string initialQuizFolder, 
            ApplicationDbContext appDbContext,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            Options.IdentityOptions identityOptions)
        {
            var superAdminUser = await userManager.FindByNameAsync(identityOptions.SuperAdminDefaultUserName);

            if (superAdminUser == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = identityOptions.SuperAdminDefaultUserName,
                    Email = identityOptions.SuperAdminDefaultUserName
                };

                var role = new ApplicationRole()
                {
                    Name = IdentityConstants.SuperAdministratorRoleName
                };

                await userManager.CreateAsync(user, identityOptions.SuperAdminDefaultPassword);
                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, role.Name);
            }

            await AddInitialSettingsAsync(appDbContext);
        }

        private static async Task AddInitialSettingsAsync(ApplicationDbContext appDbContext)
        {
            var initialSettings = appDbContext.ApplicationSettings.ToList();

            if (initialSettings.FirstOrDefault(x => x.Key == "Sessions.RecommendedSessionCountPerDay") == null)
            {
                ApplicationSetting setting = new ApplicationSetting()
                {
                    ApplicationSettingValueType = ApplicationSettingValueType.Int,
                    Key = "Sessions.RecommendedSessionCountPerDay",
                    Name = "Recommend Sessions Per Day",
                    Value = "10"
                };

                appDbContext.ApplicationSettings.Add(setting);
            }

            if (initialSettings.FirstOrDefault(x => x.Key == "Quiz.PassingGrade") == null)
            {
                ApplicationSetting setting = new ApplicationSetting()
                {
                    ApplicationSettingValueType = ApplicationSettingValueType.Double,
                    Key = "Quiz.PassingGrade",
                    Name = "Passing Grade",
                    Value = "70"
                };

                appDbContext.ApplicationSettings.Add(setting);
            }

            await appDbContext.SaveChangesAsync();
        }
    }
}
