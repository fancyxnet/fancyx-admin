using System.Data;

namespace Fancyx.EfCore
{
    public interface IUnitOfWorkManager
    {
        Task<IUnitOfWork> BeginAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}