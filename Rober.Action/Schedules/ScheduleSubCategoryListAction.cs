using Rober.Action.Model.Schedules;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Schedules
{
    public class ScheduleSubCategoryListAction : ActionSupport<ScheduleSubCategoryListRequest, ScheduleSubCategoryListResponse>
    {
        #region 构造函数
        private readonly IScheduleSubCategoryRepository _scheduleSubCategoryServer;
        public ScheduleSubCategoryListAction(IScheduleSubCategoryRepository scheduleSubCategoryServer)
        {
            this._scheduleSubCategoryServer = scheduleSubCategoryServer;
        } 
        #endregion

        public override int DoExecute(ScheduleSubCategoryListRequest request, RequestHeader requestHeader, out ScheduleSubCategoryListResponse response)
        {
            response = new ScheduleSubCategoryListResponse();
            if (request.PageSize == 0)
            {
                var list = _scheduleSubCategoryServer.GetList();
                response.PagedList = list;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            else
            {
                var list = _scheduleSubCategoryServer.GetList(scheduleCategory: request.ScheduleCategory, pageIndex: request.PageIndex, pageSize: request.PageSize);
                response.PagedList = list;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            SetSuccess();
            return Code;
        }
    }
}
