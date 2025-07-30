namespace Fancyx.Payment.Exceptions
{
    public class PayCallBackException : Exception
    {
        public PayCallBackException(string message) : base(message)
        {
        }

        public PayCallBackException(bool isValid, string paramName) : base($"回调参数{paramName}不存在")
        {
        }
    }
}