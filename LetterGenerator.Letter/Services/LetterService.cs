using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Models;
using LetterGenerator.Letter.Types;
using LetterGenerator.Shared.Exceptions;
using LetterGenerator.Shared.Maps;
using LetterGenerator.Shared.Types;
using Microsoft.Extensions.Logging;

namespace LetterGenerator.Letter.Services
{
    public class LetterService : ILetterService
    {
        private readonly ILetterRepository _repository;
        private readonly ILetterSyncAdapter _adapter;
        private readonly ILogger<LetterService> _logger;

        public LetterService(ILetterRepository repository, ILetterSyncAdapter adapter, ILogger<LetterService> logger)
        {
            _repository = repository;
            _adapter = adapter;
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

        public async Task<bool> CreateAsync(CreateLetterDto dto)
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

                var result = await _repository.CreateAsync(letter);

                if (result > 0)
                {
                    var syncStatus = new LetterSyncStatus
                    {
                        LetterId = letter.Id,
                        Username = dto.Username,
                        DeviceType = dto.DeviceType,
                        IsSynced = false,
                        SyncType = Types.SyncType.Create,
                        LastCheckedDateTimeUtc = DateTime.UtcNow
                    };

                    var retValue = await _repository.CreateSyncStatusAsync(syncStatus);

                    if (retValue > 0)
                    {
                        return true;
                    }
                }

                return false;
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

                var result = await _repository.UpdateAsync(letter);

                if (result)
                {
                    var syncStatus = new LetterSyncStatus
                    {
                        LetterId = letter.Id,
                        Username = dto.Username,
                        DeviceType = dto.DeviceType,
                        IsSynced = false,
                        SyncType = Types.SyncType.Update,
                        LastCheckedDateTimeUtc = DateTime.UtcNow
                    };
                    var retValue = await _repository.UpdateSyncStatusAsync(syncStatus);

                    if (retValue)
                    {
                        return true;
                    }
                }

                return false;
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

        public async Task<bool> SyncLettersAsync(string username, DeviceType deviceType)
        {
            var unyncedLetters = await _repository.GetUnsyncedLettersAsync(username, deviceType);

            foreach (var status in unyncedLetters)
            {
                var letter = status.Letter;
                switch (status.SyncType)
                {
                    case SyncType.Create:
                        await _adapter.SendCreateAsync(letter);
                        break;
                    case SyncType.Update:
                        await _adapter.SendUpdateAsync(letter);
                        break;
                    case SyncType.Delete:
                        await _adapter.SendDeleteAsync(letter.Id);
                        break;
                }

                status.IsSynced = true;
                status.LastCheckedDateTimeUtc = DateTime.UtcNow;
                await _repository.UpdateSyncStatusAsync(status);
            }

            return true;
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