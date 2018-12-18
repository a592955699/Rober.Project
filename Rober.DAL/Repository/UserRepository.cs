using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rober.Core;
using Rober.Core.Domain.Account;
using Rober.IDAL;
using Rober.IDAL.Repository;


namespace Rober.DAL.Repository
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        #region 构造函数
        private readonly IUserRuleMappingRepository _userRuleMappingRepository;
        private readonly IRuleRepository _ruleRepository;
        private readonly IUserDepartmentMappingRepository _userDepartmentMappingRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public UserRepository(IEfUnitOfWork efUnitOfWork
            , IUserRuleMappingRepository userRuleMappingRepository
            , IUserDepartmentMappingRepository userDepartmentMappingRepository
            , IRuleRepository ruleRepository
            , IDepartmentRepository departmentRepository
        ) : base(efUnitOfWork)
        {
            _userRuleMappingRepository = userRuleMappingRepository;
            _userDepartmentMappingRepository = userDepartmentMappingRepository;
            _ruleRepository = ruleRepository;
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region 公有方法
        public virtual bool InsertAll(User user, bool IsCommit = true)
        {
            base.Insert(user, false);
            #region 新增角色
            if (user.Roles != null && user.Roles.Any())
            {
                var ruleMappings = user.Roles.Select(x => new UserRuleMapping()
                {
                    UserId = user.Id,
                    RuleId = x.Id
                });
                _userRuleMappingRepository.Insert(ruleMappings, false);
            }
            #endregion

            #region 新增部门
            if (user.Departments != null && user.Departments.Any())
            {
                var ruleMappings = user.Departments.Select(x => new UserDepartmentMapping()
                {
                    UserId = user.Id,
                    DepartmentId = x.Id
                });
                _userDepartmentMappingRepository.Insert(ruleMappings, false);
            }
            #endregion
            return IsCommit && UnitOfWork.Commit() > 0;
        }

        public virtual bool UpdateAll(User user, bool IsCommit = true)
        {
            //删除角色
            var rules = _userRuleMappingRepository.Table.Where(x => x.UserId == user.Id);
            _userRuleMappingRepository.Delete(rules, false);

            //删除部门
            var deps = _userDepartmentMappingRepository.Table.Where(x => x.UserId == user.Id);
            _userDepartmentMappingRepository.Delete(deps, false);


            #region 新增角色
            if (user.Roles != null && user.Roles.Any())
            {
                var ruleMappings = user.Roles.Select(x => new UserRuleMapping()
                {
                    UserId = user.Id,
                    RuleId = x.Id
                });
                _userRuleMappingRepository.Insert(ruleMappings, false);
            }
            #endregion

            #region 新增部门
            if (user.Departments != null && user.Departments.Any())
            {
                var ruleMappings = user.Departments.Select(x => new UserDepartmentMapping()
                {
                    UserId = user.Id,
                    DepartmentId = x.Id
                });
                _userDepartmentMappingRepository.Insert(ruleMappings, false);
            }
            #endregion

            base.Update(user, false);
            return IsCommit && UnitOfWork.Commit() > 0;
        }

        public User GetByUserName(string userName)
        {
            return Get(x => x.UserName.ToUpper() == userName.ToUpper());
        }

        public PagedList<User> GetList(IEnumerable<int> userIds = null, int ruleId = 0, string userName = "", string nickName = "", bool? publiched = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from u in Table
                        select u;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(x => x.UserName.Contains(userName));
            }

            if (!string.IsNullOrWhiteSpace(nickName))
            {
                query = query.Where(x => x.NickName.Contains(nickName));
            }
            if (ruleId != 0)
            {
                query = from urm in _userRuleMappingRepository.Table
                        join u in query on urm.UserId equals u.Id
                        where urm.RuleId == ruleId
                        select u;

            }
            if (publiched.HasValue)
            {
                query = query.Where(x => x.Published == publiched);
            }

            if (userIds != null && userIds.Any())
            {
                query = from p in query
                        join u in userIds
                            on p.Id equals u
                        select p;
            }
            return new PagedList<User>(query, pageIndex, pageSize);
        }

        public PagedList<User> GetOnlineList(IEnumerable<int> userIds, string userName = "", string nickName = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (userIds == null)
                return new PagedList<User>();
            
            var query = from u in Table
                        join n in userIds on u.Id equals n
                        select u;

            if (!string.IsNullOrWhiteSpace(nickName))
            {
                query = query.Where(x => x.NickName.ToUpper().Contains(nickName));
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(x => x.UserName.ToUpper().Contains(userName));
            }
            return new PagedList<User>(query, pageIndex, pageSize);
        }

        #region 删除rule
        public bool DestroyUserRuleMapping(IEnumerable<int> userIds)
        {
            var mappings = _userRuleMappingRepository.Table.Where(x => userIds.Contains(x.UserId)).ToList();

            return _userRuleMappingRepository.Delete(mappings);
        }
        public bool DestroyUserRuleMapping(int userId, int ruleId)
        {
            var mapping = _userRuleMappingRepository.TableNoTracking.FirstOrDefault(x => x.UserId == userId && x.RuleId == ruleId);
            return _userRuleMappingRepository.Delete(mapping);
        }
        public bool DestroyUserRuleMapping(int userId)
        {
            var mappings = _userRuleMappingRepository.Table.Where(x => x.UserId == userId).ToList();

            return _userRuleMappingRepository.Delete(mappings);
        }
        #endregion

        #region 删除部门
        public bool DestroyUserDepartmentMapping(int userId, int departmentId)
        {
            var mapping = _userDepartmentMappingRepository.Table.FirstOrDefault(x => x.UserId == userId && x.DepartmentId == departmentId);
            return _userDepartmentMappingRepository.Delete(mapping);
        }
        public bool DestroyUserDepartmentMapping(IEnumerable<int> userIds)
        {
            var mappings = _userDepartmentMappingRepository.Table.Where(x => userIds.Contains(x.UserId)).ToList();

            return _userDepartmentMappingRepository.Delete(mappings);
        }
        public bool DestroyUserDepartmentMapping(int userId)
        {
            var mappings = _userDepartmentMappingRepository.Table.Where(x => x.UserId == userId).ToList();

            return _userDepartmentMappingRepository.Delete(mappings);
        }
        #endregion

        public List<Rule> GetUseRules(int userId)
        {
            var query = from r in _ruleRepository.Table
                        join d in _userRuleMappingRepository.Table
                            on r.Id equals d.RuleId
                        where d.UserId == userId
                        select new Rule()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Published = r.Published,
                            Remark = r.Remark
                        };
            return query.ToList();
        }

        public List<Department> GetUseDepartments(int userId)
        {
            var query = from r in _departmentRepository.Table
                        join d in _userDepartmentMappingRepository.Table
                            on r.Id equals d.DepartmentId
                        where d.UserId == userId
                        select new Department()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Remark = r.Remark
                        };

            return query.ToList();
        }
        #endregion
    }
}
