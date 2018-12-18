using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Common;
using Rober.IDAL;
using Rober.IDAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rober.DAL.Repository
{
    public class GenericAttributeRepository : EfRepository<GenericAttribute>, IGenericAttributeRepository
    {
        #region 构造函数
        public GenericAttributeRepository(IEfUnitOfWork efUnitOfWork) : base(efUnitOfWork)
        {
        }
        #endregion

        #region 公有方法
        public GenericAttribute GetAttributeForEntity(int entityId, string keyGroup, string key)
        {
            var query = from ga in Table
                        where ga.EntityId == entityId &&
                              ga.KeyGroup == keyGroup &&
                              ga.Key == key
                        select ga;
            return query.FirstOrDefault();
        }

        public List<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup)
        {
            var query = from ga in Table
                        where ga.EntityId == entityId &&
                              ga.KeyGroup == keyGroup
                        select ga;
            var attributes = query.ToList();
            return attributes;
        }

        public void SaveAttribute<TPropType>(int entityId, string keyGroup, string key, TPropType value)
        {
            if (entityId == 0)
                throw new ArgumentNullException(nameof(entityId));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(keyGroup))
                throw new ArgumentNullException(nameof(keyGroup));


            var props = GetAttributesForEntity(entityId, keyGroup)
                .ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    Delete(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    Update(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute
                    {
                        EntityId = entityId,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr,

                    };
                    Insert(prop);
                }
            }
        }
        #endregion
    }
}
