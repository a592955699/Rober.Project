using System.Collections.Generic;
using Rober.Core.Plugins;

namespace Rober.Core.Plugins
{
    /// <summary>
    /// Plugins uploaded event
    /// </summary>
    public class PluginsUploadedEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="uploadedPlugins">Uploaded plugins</param>
        public PluginsUploadedEvent(IList<PluginDescriptor> uploadedPlugins)
        {
            this.UploadedPlugins = uploadedPlugins;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Uploaded plugins
        /// </summary>
        public IList<PluginDescriptor> UploadedPlugins { get; private set; }

        #endregion
    }
}