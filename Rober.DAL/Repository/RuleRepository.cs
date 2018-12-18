using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.IDAL;
using Rober.IDAL.Repository;


namespace Rober.DAL.Repository
{
    public class RuleRepository : EfRepository<Rule>, IRuleRepository
    {
        #region 构造函数
        private readonly IUserRuleMappingRepository _userRuleMappingRepository;
        public RuleRepository(IEfUnitOfWork efUnitOfWork
            , IUserRuleMappingRepository userRuleMappingRepository) : base(efUnitOfWork)
        {
            _userRuleMappingRepository = userRuleMappingRepository;
        }
        #endregion

        #region 公有方法
        public PagedList<Rule> GetList(int id = 0, string name = "", bool? publiched = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = TableNoTracking;
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name.TrimEnd()));
            }

            if (id != 0)
            {
                query = query.Where(x => x.Id == id);
            }
            if (publiched.HasValue)
            {
                query = query.Where(x => x.Published == publiched);
            }
            return new PagedList<Rule>(query, pageIndex, pageSize);
        }

        public bool RuleUserCreate(int ruleId, int[] userIds)
        {
            var mapingList = userIds.GroupBy(x => x).Select(x => new UserRuleMapping() { RuleId = ruleId, UserId = x.Key });

            return _userRuleMappingRepository.Insert(mapingList);
        }
        #endregion
    }
}
