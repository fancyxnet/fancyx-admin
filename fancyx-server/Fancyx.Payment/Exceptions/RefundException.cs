namespace Fancyx.Payment.Exceptions
{
    public class RefundException : Exception
    {
        public RefundException(string message) : base(message)
        {
        }
    }
}