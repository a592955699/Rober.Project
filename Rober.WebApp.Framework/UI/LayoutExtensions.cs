
using Microsoft.AspNetCore.Mvc.Rendering;
using Rober.Core.Infrastructure;

namespace Rober.WebApp.Framework.UI
{
    /// <summary>
    /// Layout extensions
    /// </summary>
    public static class LayoutExtensions
    {
       
        /// <summary>
        /// Specify system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="systemName">System name</param>
        public static void SetActiveMenuItemSystemName(this IHtmlHelper html, string systemName)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.SetActiveMenuItemSystemName(systemName);
        }
        /// <summary>
        /// Get system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <returns>System name</returns>
        public static string GetActiveMenuItemSystemName(this IHtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return pageHeadBuilder.GetActiveMenuItemSystemName();
        }
    }
}