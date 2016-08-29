using QuizMaster.Data.Services;
using System.Threading.Tasks;

namespace QuizMaster.Data.Settings
{
    public class SessionSettings : SettingsBase
    {
        public SessionSettings(ApplicationSettingService appSettingService) : base(appSettingService)
        {
        }

        public Task<int> RecommendedSessionCountPerDay
        {
            get
            {
                return SettingService.GetIntValueAsync("Sessions.RecommendedSessionCountPerDay");
            }
        }
    }
}
