using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Data.Core;
using QuizMaker.Data.Repositories;
using QuizMaster.Data.Services;
using QuizMaster.Data.Settings;

namespace QuizMaster.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<QuizService>();
            services.AddScoped<ApplicationSettingService>();
        }

        public static void AddApplicationSettings(this IServiceCollection services)
        {
            services.AddScoped<SessionSettings>();
            services.AddScoped<QuizSettings>();
        }

        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<SessionRepository>();
        }
    }
}
