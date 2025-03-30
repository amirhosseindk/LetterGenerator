using LetterGenerator.Letter.Models;

namespace LetterGenerator.Letter.Contracts
{
    public interface ILetterService
    {
        Task<LetterDto> GetByIdAsync(Guid id);
        Task<LetterDto> GetByNumberAsync(string number);
        Task<IEnumerable<LetterDto>> GetAllAsync();
        Task<int> CreateAsync(CreateLetterDto dto);
        Task<bool> UpdateAsync(UpdateLetterDto dto);
        Task<bool> SoftDeleteAsync(Guid id, string username);
        Task<bool> DeleteAsync(Guid id);
    }
}