using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.IDAL
{
    /// <summary>
    /// 表示EF的工作单元接口，因为DbContext是EF的对象
    /// </summary>
    public interface IEfUnitOfWork : IUnitOfWorkRepositoryContext
    {
        IDbContext Context { get; }
    }
}
