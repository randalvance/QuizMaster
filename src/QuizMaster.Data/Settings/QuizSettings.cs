using QuizMaster.Data.Services;
using System.Threading.Tasks;

namespace QuizMaster.Data.Settings
{
    public class QuizSettings : SettingsBase
    {
        public QuizSettings(ApplicationSettingService appSettingService) : base(appSettingService)
        {
        }

        public Task<double> PassingGrade
        {
            get
            {
                return SettingService.GetDoubleValueAsync("Quiz.PassingGrade");
            }
        }
    }
}
