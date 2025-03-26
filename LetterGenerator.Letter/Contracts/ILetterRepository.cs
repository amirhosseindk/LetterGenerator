namespace LetterGenerator.Letter.Contracts
{
    public interface ILetterRepository
    {
        Task<Models.Letter> GetByIdAsync(Guid id);
        Task<Models.Letter> GetByNumberAsync(string number);
        Task<IEnumerable<Models.Letter>> GetAllAsync();
        Task<int> CreateAsync(Models.Letter letter);
        Task<bool> UpdateAsync(Models.Letter letter);
        Task<bool> DeleteAsync(Guid id);
    }
}