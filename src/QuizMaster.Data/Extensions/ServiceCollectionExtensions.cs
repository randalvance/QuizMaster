using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Data.Core;
using QuizMaster.Data.Services;
using QuizMaster.Data.Settings;

namespace QuizMaster.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<QuizService>();
            services.AddTransient<ApplicationSettingService>();
        }

        public static void AddApplicationSettings(this IServiceCollection services)
        {
            services.AddSingleton<SessionSettings>();
            services.AddSingleton<QuizSettings>();
        }
    }
}
