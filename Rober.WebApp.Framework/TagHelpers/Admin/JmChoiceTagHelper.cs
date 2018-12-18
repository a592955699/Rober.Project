using System;
using System.Collections.Generic;
using Rober.WebApp.Framework.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Rober.WebApp.Framework.TagHelpers.Admin
{
    /// <summary>
    /// nop-select tag helper
    /// </summary>
    [HtmlTargetElement("jm-choice", TagStructure = TagStructure.WithoutEndTag)]
    public class JmChoiceTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string UrlAttributeName = "asp-url";
        private const string HeightAttributeName = "asp-height";
        private const string WidthAttributeName = "asp-width";
        private const string NameAttributeName = "asp-for-name";
        private const string ItemsAttributeName = "asp-items";
        private const string DisabledAttributeName = "asp-multiple";
        private const string RequiredAttributeName = "asp-required";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Name for a dropdown list
        /// </summary>
        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        [HtmlAttributeName(UrlAttributeName)]
        public string Url { get; set; }

        [HtmlAttributeName(HeightAttributeName)]
        public int Height { get; set; } = 600;

        [HtmlAttributeName(WidthAttributeName)]
        public int Width { get; set; } = 800;

        /// <summary>
        /// Items for a dropdown list
        /// </summary>
        [HtmlAttributeName(ItemsAttributeName)]
        public IEnumerable<SelectListItem> Items { set; get; } = new List<SelectListItem>();

        /// <summary>
        /// Indicates whether the field is required
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public string IsRequired { set; get; }

        /// <summary>
        /// Indicates whether the input is multiple
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public string IsMultiple { set; get; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        public JmChoiceTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

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
            var tagName = For != null ? For.Name : Name;
            //clear the output
            output.SuppressOutput();

            //required asterisk
            bool.TryParse(IsRequired, out bool required);
            if (required || !string.IsNullOrWhiteSpace(Url))
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                string content = "<div class=\"input-group-btn\">";
                if (!string.IsNullOrWhiteSpace(Url))
                {
                    Url = Url + (Url.Contains("?")?"&":"?") + "ChoiceId=" + tagName;
                    content += "<input type=\"button\" value=\"选择...\" onclick=\"javascript: OpenWindow('" + Url + "', " + Width + ", " + Height + ", true); return false; \" />";
                }
                if (required)
                {
                    content+="<span class=\"required\">*</span>";
                }
                content +="</div></div>";
                output.PostElement.SetHtmlContent(content);
            }

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //get htmlAttributes object
            var htmlAttributes = new Dictionary<string, object>();
            var attributes = context.AllAttributes;
            foreach (var attribute in attributes)
            {
                if (!attribute.Name.Equals(ForAttributeName) &&
                    !attribute.Name.Equals(NameAttributeName) &&
                    !attribute.Name.Equals(ItemsAttributeName) &&
                    !attribute.Name.Equals(DisabledAttributeName) &&
                    !attribute.Name.Equals(RequiredAttributeName))
                {
                    htmlAttributes.Add(attribute.Name, attribute.Value);
                }
            }

            //generate editor
            //var tagName = For != null ? For.Name : Name;
            bool.TryParse(IsMultiple, out bool multiple);
            if (!string.IsNullOrEmpty(tagName))
            {
                IHtmlContent selectList;
                if (multiple)
                {
                    selectList = _htmlHelper.Editor(tagName, "MultiChoice", new { htmlAttributes, SelectList = Items, Url = Url, Height = Height, Width = Width });
                }
                else
                {
                    if (htmlAttributes.ContainsKey("class"))
                        htmlAttributes["class"] += " form-control";
                    else
                        htmlAttributes.Add("class", "form-control");

                    selectList = _htmlHelper.DropDownList(tagName, Items, htmlAttributes);
                }
                output.Content.SetHtmlContent(selectList.RenderHtmlContent());
            }
        }
    }
}