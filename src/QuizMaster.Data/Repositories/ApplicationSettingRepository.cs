using Microsoft.EntityFrameworkCore;
using QuizMaster.Data;
using QuizMaster.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Data.Repositories
{
    public class ApplicationSettingRepository : BaseRepository<ApplicationSetting>
    {
        public ApplicationSettingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ApplicationSetting> FindByKeyAsync(string key)
        {
            var appSetting = await DbContext.ApplicationSettings.SingleOrDefaultAsync(x => x.Key == key);

            return appSetting;
        }
    }
}
