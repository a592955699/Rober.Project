using Rober.Core;
using Rober.Core.Domain.Account;
using System;
using System.Collections.Generic;

namespace Rober.IDAL.Repository
{
    public partial interface IUserRepository : IRepository<User>
    {
        User GetByUserName(string userName);
        PagedList<User> GetList(IEnumerable<int> userIds = null, int ruleId = 0, string userName = "", string nickName = "", bool? publiched = null, int pageIndex = 0, int pageSize = int.MaxValue);
        PagedList<User> GetOnlineList(IEnumerable<int> userIds, string userName = "", string nickName = "", int pageIndex = 0, int pageSize = Int32.MaxValue);

        bool UpdateAll(User entity, bool IsCommit = true);
        bool InsertAll(User entity, bool IsCommit = true);

        #region 删除rule
        bool DestroyUserRuleMapping(int userId, int ruleId);
        bool DestroyUserRuleMapping(IEnumerable<int> userIds);
        bool DestroyUserRuleMapping(int userId);
        #endregion

        #region 删除部门
        bool DestroyUserDepartmentMapping(int userId, int departmentId);
        bool DestroyUserDepartmentMapping(IEnumerable<int> userIds);
        bool DestroyUserDepartmentMapping(int userId);
        #endregion

        #region 获取用户rule
        List<Rule> GetUseRules(int userId);
        #endregion
        #region 获取用户Department
        List<Department> GetUseDepartments(int userId);
        #endregion
    }
}
