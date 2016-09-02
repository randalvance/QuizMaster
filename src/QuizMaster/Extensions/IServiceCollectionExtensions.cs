using QuizMaster.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISortManager, SortManager>();
        }
    }
}
