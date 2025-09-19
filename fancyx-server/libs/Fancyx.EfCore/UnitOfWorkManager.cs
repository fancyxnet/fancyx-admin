using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Fancyx.EfCore
{
    internal class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly DbContext _context;

        public UnitOfWorkManager(DbContext context)
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