
namespace Rober.WebApp.Framework.MVC.ModelBinding
{
    /// <summary>
    /// Represents custom model attribute
    /// </summary>
    public interface IModelAttribute
    {
        /// <summary>
        /// Gets name of the attribute
        /// </summary>
        string Name { get; }
    }
}
