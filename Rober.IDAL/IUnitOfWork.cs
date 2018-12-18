using System;
using System.Collections.Generic;
using System.Text;

namespace Rober.IDAL
{
    /// <summary>
    /// Describe：工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        bool IsCommitted { get; set; }

        int Commit();

        void Rollback();
    }
}
