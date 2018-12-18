using System;

namespace Rober.Core.Domain.Common
{
    public class CustomerFile : BaseEntity
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
