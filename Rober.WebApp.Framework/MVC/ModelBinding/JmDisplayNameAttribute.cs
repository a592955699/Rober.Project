using System.ComponentModel;

namespace Rober.WebApp.Framework.MVC.ModelBinding
{
    /// <summary>
    /// Represents model attribute that specifies the display name by passed key of the locale resource
    /// </summary>
    public class JmDisplayNameAttribute : DisplayNameAttribute, IModelAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the attribute
        /// </summary>
        /// <param name="displayName">Key of the locale resource</param>
        public JmDisplayNameAttribute(string displayName) : base(displayName)
        {
      
        }

        /// <summary>
        /// Create instance of the attribute
        /// </summary>
        /// <param name="displayName">Key of the locale resource</param>
        public JmDisplayNameAttribute(string displayName,string hint) : base(displayName)
        {
            this.Hint = hint;
        }


        #endregion

        #region Properties



        /// <summary>
        /// Gets name of the attribute
        /// </summary>
        public string Name => nameof(JmDisplayNameAttribute);

        public string Hint { get; set; }

        #endregion
    }
}
