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

        public async Task<IUnitOfWork> BeginAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return new UnitOfWork(Guid.NewGuid(), transaction);
        }

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}