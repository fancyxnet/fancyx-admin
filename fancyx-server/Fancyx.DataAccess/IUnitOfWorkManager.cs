using System.Transactions;

namespace Fancyx.DataAccess
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}