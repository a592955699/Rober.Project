using Rober.Core;
using Rober.Core.Action;
using Rober.Core.Domain.Schedules;

namespace Rober.Action.Model.Schedules
{
    public class ScheduleSubCategoryListResponse : Response
    {
        public PagedList<ScheduleSubCategory> PagedList { get; set; }
        //public int TotalPages { get; set; }
        //public int TotalCount { get; set; }
    }
}
