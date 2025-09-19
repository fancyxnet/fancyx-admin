using Fancyx.Core.AutoInject;
using Fancyx.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.EfCore.Aop
{
    /// <summary>
    /// 自动将代码块放在事务中执行，异常自动回滚（只适合异步方法）
    /// </summary>
    public class AsyncTransactionalAttribute : AsyncAopAttributeBase
    {
        private IUnitOfWork? _uow;

        public AsyncTransactionalAttribute() : base(true)
        {
        }

        public override async Task OnAfterAsync()
        {
            if (_uow != null)
            {
                await _uow.CommitAsync();
                await _uow.DisposeAsync();
            }
        }

        public override async Task OnBeforeAsync()
        {
            var unitOfWorkManager = ServiceProvider.GetService<IUnitOfWorkManager>();
            if (unitOfWorkManager != null)
            {
                _uow = await unitOfWorkManager.BeginAsync();
            }
        }

        public override async Task OnExceptionAsync()
        {
            if(_uow != null)
            {
                await _uow.RollbackAsync();
                await _uow.DisposeAsync();
            }
        }
    }
}