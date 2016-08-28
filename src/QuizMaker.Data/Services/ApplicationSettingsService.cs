using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Data.Services
{
    public class ApplicationSettingService : ServiceBase
    {
        public ApplicationSettingService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext, userManager)
        {
        }

        public async Task<string> GetStringValueAsync(string name)
        {
            return (await GetValueAsync(name)).ToString();
        }

        public async Task<int> GetIntValueAsync(string name)
        {
            return (int)(await GetValueAsync(name));
        }

        public async Task<double> GetDoubleValueAsync(string name)
        {
            return (double)(await GetValueAsync(name));
        }

        public async Task<bool> GetBoolValueAsync(string name)
        {
            return (bool)(await GetValueAsync(name));
        }

        public async Task<Guid> GetGuidValueAsync(string name)
        {
            return (Guid)(await GetValueAsync(name));
        }

        public async Task<object> GetValueAsync(string name)
        {
            var appSetting = await DbContext.ApplicationSettings.SingleOrDefaultAsync(setting => setting.Name == name);

            if (appSetting == null)
            {
                throw new InvalidOperationException($"The AppSetting {name} cannot be found.");
            }

            switch(appSetting.ApplicationSettingValueType)
            {
                case ApplicationSettingValueType.Int:
                    return int.Parse(appSetting.Value);
                case ApplicationSettingValueType.Double:
                    return double.Parse(appSetting.Value);
                case ApplicationSettingValueType.Boolean:
                    return bool.Parse(appSetting.Value);
                case ApplicationSettingValueType.Guid:
                    return Guid.Parse(appSetting.Value);
                default:
                    return appSetting.Value;
            }
        }
    }
}
