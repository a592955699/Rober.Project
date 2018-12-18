namespace Rober.Core.Action
{
    public class SessionRequest : Request
    {
        public SessionHead SessionHead { get; set; }
    }
    public class SessionPageRequest : SessionRequest
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = int.MaxValue;
    }
    public class PageRequest : Request
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = int.MaxValue;
    }
}
