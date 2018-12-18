using Rober.Action.Model.Schedules;
using Rober.Core.Action;
using Rober.Core.Session;
using Rober.IDAL.Repository;

namespace Rober.Action.Schedules
{
    public class ScheduleMySchedulerAction : ActionSupport<ScheduleMySchedulerRequest, ScheduleMySchedulerResponse>
    {
        #region 构造函数
        private readonly IScheduleRepository _scheduleServer;
        public ScheduleMySchedulerAction(IScheduleRepository scheduleServer)
        {
            this._scheduleServer = scheduleServer;
        }
        #endregion

        public override int DoExecute(ScheduleMySchedulerRequest request, RequestHeader requestHeader, out ScheduleMySchedulerResponse response)
        {
            response = new ScheduleMySchedulerResponse();
            var list = _scheduleServer.GetList(SessionValueObject.Current.User.Id, request.Start, request.End);
            response.List = list;
            SetSuccess();
            return Code;
        }
    }
}
