using System;
using System.Collections.Generic;
using System.Linq;

namespace Rober.WebApp.Framework.MenuExtensions
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks whether this node or child ones has a specified system name
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="systemName">System name</param>
        /// <returns>Result</returns>
        public static bool ContainsSystemName(this MenuNode node, string systemName)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (string.IsNullOrWhiteSpace(systemName))
                return false;

            if (systemName.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return node.ChildNodes.Any(cn => ContainsSystemName(cn, systemName));
        }

        public static List<MenuNode> Convert(this IEnumerable<Core.Domain.Account.Menu> menus, int parentId = 0)
        {
            return menus.Where(x => x.ParentId == parentId).Select(x => new MenuNode()
            {
                Id=x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                Icon = x.Icon,
                Link=x.Link,
                Script=x.Script,
                Remark=x.Remark,
                Order =x.Order,
                OpenUrlInNewTab = x.OpenUrlInNewTab,
                IsMenu=x.IsMenu,
                Published=x.Published,
                ChildNodes = menus.Convert(x.Id)
            }).ToList();
        }
    }
}
