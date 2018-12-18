namespace Rober.WebApp.Framework.Kendoui
{
    /// <summary>
    /// DataSource request
    /// </summary>
    public class DataSourceRequest
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public DataSourceRequest()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        /// <summary>
        /// Page number,从1开始
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }
    }
}
