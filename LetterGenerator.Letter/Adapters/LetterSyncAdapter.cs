using LetterGenerator.Letter.Contracts;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LetterGenerator.Letter.Adapters
{
    public class LetterSyncAdapter : ILetterSyncAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LetterSyncAdapter> _logger;

        public LetterSyncAdapter(HttpClient httpClient, ILogger<LetterSyncAdapter> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> SendCreateAsync(Models.Letter letter)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/letters", letter);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send create letter request");
                return false;
            }
        }

        public async Task<bool> SendUpdateAsync(Models.Letter letter)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/letters/{letter.Id}", letter);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send update letter request");
                return false;
            }
        }

        public async Task<bool> SendDeleteAsync(Guid letterId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/letters/{letterId}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send delete letter request");
                return false;
            }
        }
    }
}