using Rober.Core.Domain.Common;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IGenericAttributeRepository : IRepository<GenericAttribute>
    {
        /// <summary>
        /// Get attribute
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <param name="key"></param>
        /// <returns>Get attributes</returns>
        GenericAttribute GetAttributeForEntity(int entityId, string keyGroup, string key);
        /// <summary>
        /// Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        List<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup);

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="keyGroup"></param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="entityId"></param>
        void SaveAttribute<TPropType>(int entityId, string keyGroup, string key, TPropType value);
    }
}
