namespace LetterGenerator.Shared.Exceptions
{
    public class BusinessException : Exception, IBusinessException
    {
        private readonly int _code;

        public BusinessException(string message, int code, bool returnDetail = true) : base(message)
        {
            _code = code;
            ReturnDetail = returnDetail;
        }

        public int GetCode() => _code;

        bool IBusinessException.ReturnDetail()
        {
            return ReturnDetail;
        }

        public bool ReturnDetail { get; }
    }
}