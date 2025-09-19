namespace Fancyx.Admin.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string? Code { get; private set; }
    }
}