using Rober.Core.Domain.Account;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Rober.Core.Session
{
    public class SessionValueObject
    {
        public SessionValueObject()
        {
        }

        public User User { get; set; }

        ///// <summary>
        ///// 菜单/按钮权限
        ///// </summary>
        //public List<Menu> Menus { get; set; }
        /// <summary>
        /// 菜单/按钮权限
        /// </summary>
        public List<int> Menus { get; set; }

        private static AsyncLocal<SessionValueObject> _current = new AsyncLocal<SessionValueObject>();
        public static SessionValueObject Current
        {
            get
            {
                return _current.Value;
            }
            set
            {
                _current.Value = value;
            }
        }
    }
}
