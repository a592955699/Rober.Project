using Rober.Action.Model.Schedules;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.IDAL.Repository;

namespace Rober.Action.Schedules
{
    public class ScheduleGetAction : ActionSupport<ScheduleGetRequest, ScheduleGetResponse>
    {
        #region 构造函数
        private readonly IScheduleRepository _scheduleServe;

        public ScheduleGetAction(IScheduleRepository scheduleServe)
        {
            this._scheduleServe = scheduleServe;
        }
        #endregion

        public override int DoExecute(ScheduleGetRequest request, RequestHeader requestHeader, out ScheduleGetResponse response)
        {
            response = new ScheduleGetResponse
            {
                Schedule = _scheduleServe.Get(request.Id)
            };
            if (response.Schedule != null)
            {
                response.Schedule.Users = _scheduleServe.GetScheduleUsers(request.Id);
                response.Schedule.Files = _scheduleServe.GetScheduleFiles(request.Id);
                SetSuccess();
                return Code;
            }
            else
            {
                SetCode(ResponseCode.DataNotFind);
                return Code;
            }
        }
    }
}
