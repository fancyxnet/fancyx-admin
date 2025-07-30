namespace Fancyx.Payment.Exceptions
{
    public class PaymentFailureException : Exception
    {
        public PaymentFailureException(string message) : base(message)
        {
        }
    }
}