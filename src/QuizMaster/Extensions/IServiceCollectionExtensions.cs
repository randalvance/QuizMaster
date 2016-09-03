using QuizMaster.BBCode;
using QuizMaster.Common;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISortManager, SortManager>();
            serviceCollection.AddScoped(services =>
                new BBCodeParser(new List<BBTag>
                {
                    new BBTag("b", "<b>", "</b>"),
                    new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                    new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                    new BBTag("rb", "<span style=\"font-weight:bold;color:red;\">", "</span>"),
                    new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
                    new BBTag("img", "<img src=\"${content}\" />", "", false, true),
                    new BBTag("quote", "<blockquote>", "</blockquote>"),
                    new BBTag("list", "<ul>", "</ul>"),
                    new BBTag("*", "<li>", "</li>", true, false),
                    new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href")),
                }));
        }
    }
}
