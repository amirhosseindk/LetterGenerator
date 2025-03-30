using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;
using LetterGenerator.Shared.Exceptions;
using LetterGenerator.Shared.Maps;
using LetterGenerator.Shared.Types;
using Microsoft.Extensions.Logging;

namespace LetterGenerator.Letter.Services
{
    public class LetterService : ILetterService
    {
        private readonly ILetterRepository _repository;
        private readonly ILogger<LetterService> _logger;

        public LetterService(ILetterRepository repository, ILogger<LetterService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<LetterDto> GetByIdAsync(Guid id)
        {
            try
            {
                var letter = await _repository.GetByIdAsync(id);
                return MapToLetterDto(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting letter by id: {LetterId} in service", id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServNotFound), (int)ErrorType.LetterServNotFound);
            }
        }

        public async Task<LetterDto> GetByNumberAsync(string number)
        {
            try
            {
                var letter = await _repository.GetByNumberAsync(number);
                return MapToLetterDto(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting letter by number: {LetterId} in service", number);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServNotFound), (int)ErrorType.LetterServNotFound);
            }
        }

        public async Task<IEnumerable<LetterDto>> GetAllAsync()
        {
            try
            {
                var letters = await _repository.GetAllAsync();
                return letters.Select(MapToLetterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all letters in service");
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServGetAllFailed), (int)ErrorType.LetterServGetAllFailed);
            }
        }

        public async Task<int> CreateAsync(CreateLetterDto dto)
        {
            try
            {
                var letter = new Models.Letter
                {
                    Number = dto.Number,
                    DateTimeLocal = dto.DateTimeLocal,
                    RecipientName = dto.RecipientName,
                    RecipientPosition = dto.RecipientPosition,
                    Body = dto.Body,
                    SenderName = dto.SenderName,
                    SenderPosition = dto.SenderPosition,
                    HaveCopy = dto.HaveCopy,
                    Copy = dto.Copy,
                    CreatedBy = dto.Username
                };

                return await _repository.CreateAsync(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating letter in service");
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServCreationFailed), (int)ErrorType.LetterServCreationFailed);
            }
        }

        public async Task<bool> UpdateAsync(UpdateLetterDto dto)
        {
            try
            {
                var letter = await _repository.GetByIdAsync(dto.Id);

                letter.Number = dto.Number;
                letter.DateTimeLocal = dto.DateTimeLocal;
                letter.RecipientName = dto.RecipientName;
                letter.RecipientPosition = dto.RecipientPosition;
                letter.Body = dto.Body;
                letter.SenderName = dto.SenderName;
                letter.SenderPosition = dto.SenderPosition;
                letter.HaveCopy = dto.HaveCopy;
                letter.Copy = dto.Copy;
                letter.ModifiedDateTimeUtc = DateTime.UtcNow;
                letter.ModifiedBy = dto.Username;

                return await _repository.UpdateAsync(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating letter with id: {LetterId} in service", dto.Id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServUpdateFailed), (int)ErrorType.LetterServUpdateFailed);
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id, string username)
        {
            try
            {
                var letter = await _repository.GetByIdAsync(id);

                letter.IsDeleted = true;
                letter.DeletedDateTimeUtc = DateTime.UtcNow;
                letter.DeletedBy = username;

                return await _repository.UpdateAsync(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while soft deleting letter with id: {LetterId} in repository", id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServSoftDeleteFailed), (int)ErrorType.LetterServSoftDeleteFailed);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting letter with id: {LetterId} in repository", id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServDeleteFailed), (int)ErrorType.LetterServDeleteFailed);
            }
        }

        private LetterDto MapToLetterDto(Models.Letter letter)
        {
            try
            {
                return new LetterDto
                {
                    Id = letter.Id,
                    Number = letter.Number,
                    DateTimeLocal = letter.DateTimeLocal,
                    RecipientName = letter.RecipientName,
                    RecipientPosition = letter.RecipientPosition,
                    Body = letter.Body,
                    SenderName = letter.SenderName,
                    SenderPosition = letter.SenderPosition,
                    HaveCopy = letter.HaveCopy,
                    Copy = letter.Copy,
                    InsertDateTimeUtc = letter.InsertDateTimeUtc,
                    ModifiedDateTimeUtc = letter.ModifiedDateTimeUtc ?? default,
                    DeletedDateTimeUtc = letter.DeletedDateTimeUtc ?? default,
                    IsDeleted = letter.IsDeleted,
                    CreatedBy = letter.CreatedBy,
                    ModifiedBy = letter.ModifiedBy,
                    DeletedBy = letter.DeletedBy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while mapping to letterdto in service");
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.LetterServMapToLetterDtoFailed), (int)ErrorType.LetterServMapToLetterDtoFailed);
            }
        }
    }
}