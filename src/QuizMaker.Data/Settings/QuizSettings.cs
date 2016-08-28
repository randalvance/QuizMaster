using QuizMaker.Data.Services;
using System.Threading.Tasks;

namespace QuizMaker.Data.Settings
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
