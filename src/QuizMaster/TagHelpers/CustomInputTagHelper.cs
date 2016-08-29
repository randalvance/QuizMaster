using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.TagHelpers
{
    [HtmlTargetElement("input", Attributes = ForAttributeName)]
    public class CustomInputTagHelper : InputTagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName("asp-is-correct")]
        public bool IsCorrect { get; set; }

        [HtmlAttributeName("asp-is-incorrect")]
        public bool IsIncorrect { get; set; }

        public CustomInputTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var classAttribute = output.Attributes["class"];

            if (IsCorrect)
            {
                output.Attributes.SetAttribute("class", classAttribute?.Value.ToString() + " correct-answer");
            }
            else if (IsIncorrect)
            {
                output.Attributes.SetAttribute("class", classAttribute?.Value.ToString() + " incorrect-answer");
            }

            base.Process(context, output);
        }
    }
}
