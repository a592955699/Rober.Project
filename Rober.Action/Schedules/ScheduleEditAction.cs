using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Schedules;
using Rober.Action.Model.Account;
using Rober.Action.Model.Schedules;
using Rober.IDAL.Repository;

namespace Rober.Action.Schedules
{
    public class ScheduleEditAction : ActionSupport<ScheduleEditRequest, ScheduleEditResponse>
    {
        #region 构造函数
        private readonly IScheduleRepository _scheduleServer;
        public ScheduleEditAction(IScheduleRepository scheduleServer)
        {
            this._scheduleServer = scheduleServer;
        }
        #endregion

        public override int DoExecute(ScheduleEditRequest request, RequestHeader requestHeader, out ScheduleEditResponse response)
        {
            response = new ScheduleEditResponse();
            if (request.Schedule.Id > 0)
            {
                #region 修改
                _scheduleServer.Update(request.Schedule, true,
                    x => x.Title, 
                    x => x.Common,
                    x => x.Content,
                    x => x.Remark,
                    x => x.StartTime,
                    x => x.EndTime,
                    x => x.SubCategoryId, 
                    x => x.StatusId,
                    x=>x.Top);
                response.Schedule = request.Schedule;
                SetSuccess();
                return Code;
                #endregion
            }
            else
            {
                #region 新增
                if (_scheduleServer.Insert(request.Schedule))
                {
                    response.Schedule = request.Schedule;
                    SetSuccess();
                    return Code;
                }
                #endregion
            }
            SetCode(ResponseCode.DataChangeFail);
            return Code;
        }
    }
}
