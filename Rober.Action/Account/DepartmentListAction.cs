using Rober.Action.Model.Account;
using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Account;
using Rober.IDAL.Repository;

namespace Rober.Action.Account
{
    public class DepartmentListAction : ActionSupport<DepartmentListRequest, DepartmentListResponse>
    {
        #region 构造函数
        private readonly IDepartmentRepository _DepartmentRepository;
        public DepartmentListAction(IDepartmentRepository departmentRepository)
        {
            this._DepartmentRepository = departmentRepository;
        } 
        #endregion

        public override int DoExecute(DepartmentListRequest request, RequestHeader requestHeader, out DepartmentListResponse response)
        {
            response = new DepartmentListResponse();
            if (request.PageSize == 0)
            {
                var list = _DepartmentRepository.GetList();
                var pageList = new PagedList<Department>(list);
                response.PagedList = pageList;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            else
            {
                var list = _DepartmentRepository.GetList(id: request.Id, name: request.Name, pageSize: request.PageSize, pageIndex: request.PageIndex);
                response.PagedList = list;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            SetSuccess();
            return Code;
        }
    }
}
