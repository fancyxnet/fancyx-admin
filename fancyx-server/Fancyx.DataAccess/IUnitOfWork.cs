namespace Fancyx.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();
    }
}