using System.Transactions;

namespace Fancyx.DataAccess
{
    public interface IUnitOfWorkManager
    {
        Task<IUnitOfWork> BeginAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task<int> SaveChangeAsync();
    }
}