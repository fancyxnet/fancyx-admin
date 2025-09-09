using Microsoft.EntityFrameworkCore.Storage;

namespace Fancyx.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextTransaction _contextTransaction;

        public UnitOfWork(Guid id, IDbContextTransaction contextTransaction)
        {
            this.Id = id;
            _contextTransaction = contextTransaction;
        }

        public Guid Id { get; private set; }

        public bool IsCompeleted { get; private set; }

        public async Task CommitAsync()
        {
            await _contextTransaction.CommitAsync();
            IsCompeleted = true;
        }

        public void Dispose()
        {
            _contextTransaction.Dispose();
            IsCompeleted = true;
        }

        public async ValueTask DisposeAsync()
        {
            await _contextTransaction.DisposeAsync();
            IsCompeleted = true;
        }

        public async Task RollbackAsync()
        {
            await _contextTransaction.RollbackAsync();
            IsCompeleted = true;
        }
    }
}