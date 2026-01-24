using Fancyx.Core.AutoInject;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.EfCore.Aop
{
    /// <summary>
    /// 自动将代码块放在事务中执行，异常自动回滚
    /// 如果方法内部使用了Dapper API，使用此注解引发报错，参见 https://mysqlconnector.net/troubleshooting/transaction-usage/
    /// </summary>
    public class TransactionalAttribute : AopAttributeBase
    {
        private IUnitOfWork? _uow;

        public TransactionalAttribute() : base(true)
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
            var unitOfWorkManager = ServiceScopeAccessor.Current.GetService<IUnitOfWorkManager>();
            if (unitOfWorkManager != null)
            {
                _uow = await unitOfWorkManager.BeginAsync();
            }
        }

        public override async Task OnExceptionAsync()
        {
            if (_uow != null)
            {
                await _uow.RollbackAsync();
                await _uow.DisposeAsync();
            }
        }
    }
}