using Fancyx.Core.AutoInject;

namespace Fancyx.DataAccess.Aop
{
    /// <summary>
    /// 自动将代码块放在事务中执行，异常自动回滚（只适合异步方法）
    /// </summary>
    public class AsyncTransactionalAttribute : AsyncAopAttributeBase
    {
        //public Propagation Propagation { get; set; } = Propagation.Required;
        //public IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;
        //private IUnitOfWork? _uow;

        public AsyncTransactionalAttribute() : base(true)
        {
        }

        public override Task OnAfterAsync()
        {
            //_uow?.Commit();
            //_uow?.Dispose();
            return Task.CompletedTask;
        }

        public override Task OnBeforeAsync()
        {
            //var unitOfWorkManager = ServiceProvider.GetService<UnitOfWorkManager>();
            //_uow = unitOfWorkManager?.Begin(Propagation, this.IsolationLevel);
            return Task.CompletedTask;
        }

        public override Task OnExceptionAsync()
        {
            //_uow?.Rollback();
            //_uow?.Dispose();
            return Task.CompletedTask;
        }
    }
}