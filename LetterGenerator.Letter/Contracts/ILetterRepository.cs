using LetterGenerator.Letter.Models;
using LetterGenerator.Shared.Types;

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
        Task<int> CreateSyncStatusAsync(LetterSyncStatus status);
        Task<bool> UpdateSyncStatusAsync(LetterSyncStatus status);
        Task<IEnumerable<LetterSyncStatus>> GetUnsyncedLettersAsync(string username, DeviceType device);
    }
}