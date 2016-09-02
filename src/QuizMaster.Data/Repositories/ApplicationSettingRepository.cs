using Microsoft.EntityFrameworkCore;
using QuizMaster.Models;
using System.Threading.Tasks;
using QuizMaster.Common;

namespace QuizMaster.Data.Repositories
{
    public class ApplicationSettingRepository : BaseRepository<ApplicationSetting>
    {
        public ApplicationSettingRepository(ApplicationDbContext dbContext, ISortManager sortApplier) : base(dbContext, sortApplier)
        {
        }

        public async Task<ApplicationSetting> FindByKeyAsync(string key)
        {
            var appSetting = await DbContext.ApplicationSettings.SingleOrDefaultAsync(x => x.Key == key);

            return appSetting;
        }
    }
}
