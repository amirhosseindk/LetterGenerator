namespace LetterGenerator.Letter.Contracts
{
    public interface ILetterSyncAdapter
    {
        Task<bool> SendCreateAsync(Models.Letter letter);
        Task<bool> SendUpdateAsync(Models.Letter letter);
        Task<bool> SendDeleteAsync(Guid letterId);
    }
}