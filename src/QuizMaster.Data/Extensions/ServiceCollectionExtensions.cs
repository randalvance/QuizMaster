using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Data.Repositories;
using QuizMaster.Data;
using QuizMaster.Data.Services;
using QuizMaster.Data.Settings;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataRelatedServices(this IServiceCollection services, string connectionString, IdentityBuilder identityBuilder)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, opt => opt.UseRowNumberForPaging()));

            identityBuilder
                .AddEntityFrameworkStores<ApplicationDbContext, Guid>()
                .AddDefaultTokenProviders();
        }

        public static void AddApplicationDataServices(this IServiceCollection services)
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
            services.AddScoped<ApplicationSettingRepository>();
            services.AddScoped<QuestionChoiceRepository>();
            services.AddScoped<QuizRepository>();
            services.AddScoped<QuizCategoryRepository>();
            services.AddScoped<QuizGroupRepository>();
            services.AddScoped<SessionAnswerRepository>();
            services.AddScoped<SessionRepository>();
        }
    }
}
