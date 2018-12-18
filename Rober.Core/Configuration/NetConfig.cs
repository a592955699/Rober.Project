using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.Core.Configuration
{
    /// <summary>
    /// 網路設定
    /// </summary>
    public class NetConfig
    {
        /// <summary>
        /// 端點名稱
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 端點位址
        /// </summary>
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// IPorxy實體型別
        /// </summary>
        public string Porxy
        {
            get;
            set;
        }
    }
}
