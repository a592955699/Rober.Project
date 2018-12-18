using Rober.Action.Model.Schedules;
using Rober.Core.Action;
using Rober.Core.Constants;
using Rober.IDAL.Repository;
using System.Linq;

namespace Rober.Action.Schedules
{
    public class ScheduleDestroyAction : ActionSupport<ScheduleDestroyRequest, ScheduleDestroyResponse>
    {
        #region 构造函数
        private readonly IScheduleRepository _scheduleServer;
        public ScheduleDestroyAction(IScheduleRepository scheduleServer)
        {
            this._scheduleServer = scheduleServer;
        }
        #endregion

        public override int DoExecute(ScheduleDestroyRequest request, RequestHeader requestHeader, out ScheduleDestroyResponse response)
        {
            response = new ScheduleDestroyResponse();
            _scheduleServer.DeleteScheduleUsers(request.Schedules.Select(x => x.Id));

            if (_scheduleServer.Delete(request.Schedules))
            {
                SetSuccess();
            }
            else
            {
                SetCode(ResponseCode.DataChangeFail);
            }
            return Code;
        }
    }
}