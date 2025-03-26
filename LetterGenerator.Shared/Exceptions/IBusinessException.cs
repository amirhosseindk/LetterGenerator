namespace LetterGenerator.Shared.Exceptions
{
    public interface IBusinessException
    {
        int GetCode();
        string Message { get; }
        bool ReturnDetail();
    }
}