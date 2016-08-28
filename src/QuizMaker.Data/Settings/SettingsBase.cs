using QuizMaker.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Data.Settings
{
    public abstract class SettingsBase
    {

        public SettingsBase(ApplicationSettingService appSettingService)
        {
            this.SettingService = appSettingService;
        }

        protected ApplicationSettingService SettingService { get; set; }
    }
}
