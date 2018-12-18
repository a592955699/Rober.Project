using Rober.Action.Model.Schedules;
using Rober.Core.Action;
using Rober.IDAL.Repository;

namespace Rober.Action.Schedules
{
    public class ScheduleListAction : ActionSupport<ScheduleListRequest, ScheduleListResponse>
    {
        #region 构造函数
        private readonly IScheduleRepository _ScheduleServer;
        public ScheduleListAction(IScheduleRepository ScheduleServer)
        {
            this._ScheduleServer = ScheduleServer;
        } 
        #endregion

        public override int DoExecute(ScheduleListRequest request, RequestHeader requestHeader, out ScheduleListResponse response)
        {
            response = new ScheduleListResponse();
            if (request.PageSize == 0)
            {
                var list = _ScheduleServer.GetList();
                response.PagedList = list;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            else
            {
                var list = _ScheduleServer.GetList(title: request.Title, statusId: request.StatusId, subCategoryId: request.SubCategoryId, scheduleCategory: request.ScheduleCategory, pageSize: request.PageSize, pageIndex: request.PageIndex);
                response.PagedList = list;
                //response.TotalPages = list.TotalPages;
                //response.TotalCount = list.TotalCount;
            }
            SetSuccess();
            return Code;
        }
    }
}
