namespace Fancyx.EfCore
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Guid Id { get; }

        bool IsCompeleted { get; }

        Task CommitAsync();

        Task RollbackAsync();
    }
}