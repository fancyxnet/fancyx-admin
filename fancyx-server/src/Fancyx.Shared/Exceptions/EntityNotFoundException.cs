namespace Fancyx.Shared.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("数据不存在")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}