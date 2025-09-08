using Microsoft.EntityFrameworkCore.Storage;

namespace Fancyx.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextTransaction _contextTransaction;

        public UnitOfWork(IDbContextTransaction contextTransaction)
        {
            _contextTransaction = contextTransaction;
        }

        public void Commit()
        {
            _contextTransaction.Commit();
        }

        public void Dispose()
        {
            _contextTransaction.Dispose();
        }

        public void Rollback()
        {
            _contextTransaction.Rollback();
        }
    }
}