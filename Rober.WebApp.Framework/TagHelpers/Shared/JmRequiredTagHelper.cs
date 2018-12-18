using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Rober.WebApp.Framework.TagHelpers.Shared
{
    /// <summary>
    /// nop-required tag helper
    /// </summary>
    [HtmlTargetElement("jm-required", TagStructure = TagStructure.WithoutEndTag)]
    public class JmRequiredTagHelper : TagHelper
    {
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //clear the output
            output.SuppressOutput();

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "required");
            output.Content.SetContent("*");
        }
    }
}