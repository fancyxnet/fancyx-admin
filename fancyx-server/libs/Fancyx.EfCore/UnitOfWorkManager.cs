using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Data;

namespace Fancyx.EfCore
{
    internal class UnitOfWorkManager : IUnitOfWorkManager, IDisposable, IAsyncDisposable
    {
        private readonly DbContext _context;
        private readonly ConcurrentBag<IUnitOfWork> _activeUnits;
        private bool _disposed;

        public UnitOfWorkManager(DbContext context)
        {
            _context = context;
            _activeUnits = new ConcurrentBag<IUnitOfWork>();
        }

        public async Task<IUnitOfWork> BeginAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = await _context.Database
                .BeginTransactionAsync(isolationLevel);

            var unitOfWork = new UnitOfWork(Guid.NewGuid(), transaction, _context);

            // 使用线程安全集合
            _activeUnits.Add(unitOfWork);

            return unitOfWork;
        }

        // 强制回滚所有未完成的工作单元且释放未释放单元
        private async Task RollbackAllAsync()
        {
            foreach (var unit in _activeUnits)
            {
                try
                {
                    await unit.DisposeAsync();
                }
                catch (ObjectDisposedException)
                {
                    // 忽略已释放的对象
                }
            }
        }

        private void RollbackAll()
        {
            foreach (var unit in _activeUnits)
            {
                try
                {
                    unit.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // 忽略已释放的对象
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                RollbackAll();

                _activeUnits.Clear();
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                await RollbackAllAsync();
                _activeUnits.Clear();
                _disposed = true;
            }
        }
    }
}