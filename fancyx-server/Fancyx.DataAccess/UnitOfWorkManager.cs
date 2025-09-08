using System.Transactions;

namespace Fancyx.DataAccess
{
    internal class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly FancyxDbContext _context;

        public UnitOfWorkManager(FancyxDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = _context.Database.BeginTransaction();
            return new UnitOfWork(transaction);
        }
    }
}