using Rober.Action.Model.Common;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Http.Context;
using Rober.Core.Infrastructure;
using Rober.WebApp.Framework.Proxy;
using System;

namespace Rober.WebApp.Framework.Extensions
{
    /// <summary>
    /// Generic attribute extensions
    /// </summary>
    public static class GenericAttributeExtensions
    {
        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity, string key)
        {
            if (entity == null) return default(TPropType);

            var proxyAction = EngineContext.Current.Resolve<IActionProxy>();
            return GetAttribute<TPropType>(entity, key, proxyAction);
        }

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="proxyAction">GenericAttributeService</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttribute<TPropType>(this BaseEntity entity,
            string key, IActionProxy proxyAction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var request = MessageFactory.CreateApiRequest<GetAttributeRequest>(HttpContext.Current);
            request.EntityId = entity.Id;
            request.KeyGroup = entity.GetType().Name;
            request.Key = key;
            return proxyAction.GetAttribute(request, out GetAttributeResponse response, out var responseCode) ? CommonHelper.To<TPropType>(response.GenericAttribute.Value) : default(TPropType);
        }
    }
}
