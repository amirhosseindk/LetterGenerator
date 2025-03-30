using LetterGenerator.Letter.Contracts;
using LetterGenerator.Shared.Exceptions;
using LetterGenerator.Shared.Maps;
using LetterGenerator.Shared.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetterGenerator.Letter.Repositories
{
    public class LetterRepository : ILetterRepository
    {
        private readonly LetterDbContext _context;
        private readonly ILogger<LetterRepository> _logger;

        public LetterRepository(LetterDbContext context, ILogger<LetterRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Models.Letter> GetByIdAsync(Guid id)
        {
            try
            {
                var letter = await _context.Letters.FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
                if (letter == null)
                    throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoNotFound), (int)ErrorType.LetterRepoNotFound);
                return letter;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting letter by id: {LetterId} in repository", id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoNotFound), (int)ErrorType.LetterRepoNotFound);
            }
        }

        public async Task<Models.Letter> GetByNumberAsync(string number)
        {
            try
            {
                var letter = await _context.Letters.AsNoTracking().FirstOrDefaultAsync(l => l.Number == number && !l.IsDeleted);
                if (letter == null)
                    throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoNotFound), (int)ErrorType.LetterRepoNotFound);
                return letter;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting letter by number: {Number} in repository", number);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoNotFound), (int)ErrorType.LetterRepoNotFound);
            }
        }

        public async Task<IEnumerable<Models.Letter>> GetAllAsync()
        {
            try
            {
                return await _context.Letters.AsNoTracking()
                    .Where(l => !l.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all letters in repository");
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoGetAllFailed), (int)ErrorType.LetterRepoGetAllFailed);
            }
        }

        public async Task<int> CreateAsync(Models.Letter letter)
        {
            try
            {
                await _context.Letters.AddAsync(letter);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating letter in repository");
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoCreationFailed), (int)ErrorType.LetterRepoCreationFailed);
            }
        }

        public async Task<bool> UpdateAsync(Models.Letter letter)
        {
            try
            {
                _context.Letters.Update(letter);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating letter with id: {LetterId} in repository", letter.Id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoUpdateFailed), (int)ErrorType.LetterRepoUpdateFailed);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var letter = await GetByIdAsync(id);
                _context.Letters.Remove(letter);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting letter with id: {LetterId} in repository", id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterRepoDeleteFailed), (int)ErrorType.LetterRepoDeleteFailed);
            }
        }
    }
}