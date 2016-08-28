using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Data.Services;
using QuizMaker.Data.Settings;

namespace QuizMaker.Data.Extensions
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
