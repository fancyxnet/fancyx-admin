using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fancyx.EfCore
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextTransaction _contextTransaction;
        private readonly DbContext _dbContext;
        private bool _disposed;

        public UnitOfWork(Guid id, IDbContextTransaction contextTransaction, DbContext dbContext)
        {
            Id = id;
            _contextTransaction = contextTransaction;
            _dbContext = dbContext;
        }

        public Guid Id { get; }
        public bool IsCompleted { get; private set; }

        public async Task CommitAsync(bool autoSaveChange = true)
        {
            if (IsCompleted) return;

            try
            {
                if (autoSaveChange)
                {
                    await _dbContext.SaveChangesAsync();
                }
                await _contextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                IsCompleted = true;
            }
        }

        public async Task RollbackAsync()
        {
            if (IsCompleted) return;

            await _contextTransaction.RollbackAsync();
            IsCompleted = true;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                // 如果没有显式提交或回滚，则回滚
                if (!IsCompleted)
                {
                    _contextTransaction.Rollback();
                    IsCompleted = true;
                }

                _contextTransaction.Dispose();
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                // 如果没有显式提交或回滚，则回滚
                if (!IsCompleted)
                {
                    await RollbackAsync();
                }

                await _contextTransaction.DisposeAsync();
                _disposed = true;
            }
        }
    }
}