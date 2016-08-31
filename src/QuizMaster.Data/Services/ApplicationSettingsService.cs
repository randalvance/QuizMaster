using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using System;
using System.Threading.Tasks;

namespace QuizMaster.Data.Services
{
    public class ApplicationSettingService : ServiceBase
    {
        public ApplicationSettingService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext, userManager)
        {
        }

        public async Task<string> GetStringValueAsync(string key)
        {
            return (await GetValueAsync(key)).ToString();
        }

        public async Task<int> GetIntValueAsync(string key)
        {
            return (int)(await GetValueAsync(key));
        }

        public async Task<double> GetDoubleValueAsync(string key)
        {
            return (double)(await GetValueAsync(key));
        }

        public async Task<bool> GetBoolValueAsync(string key)
        {
            return (bool)(await GetValueAsync(key));
        }

        public async Task<Guid> GetGuidValueAsync(string key)
        {
            return (Guid)(await GetValueAsync(key));
        }

        public async Task<object> GetValueAsync(string key)
        {
            var appSetting = await DbContext.ApplicationSettings.SingleOrDefaultAsync(setting => setting.Key == key);

            if (appSetting == null)
            {
                throw new InvalidOperationException($"The AppSetting {key} cannot be found.");
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
