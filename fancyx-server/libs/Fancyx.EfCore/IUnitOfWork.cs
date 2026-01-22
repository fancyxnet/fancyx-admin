namespace Fancyx.EfCore
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Guid Id { get; }
        bool IsCompleted { get; }
        Task CommitAsync(bool autoSaveChange = true);
        Task RollbackAsync();
    }
}