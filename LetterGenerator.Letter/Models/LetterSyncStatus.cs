using LetterGenerator.Letter.Types;
using LetterGenerator.Shared.Types;

namespace LetterGenerator.Letter.Models
{
    public class LetterSyncStatus
    {
        public Guid LetterId { get; set; }
        public Letter Letter { get; set; }
        public string Username { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public bool IsSynced { get; set; } = false;
        public SyncType SyncType { get; set; }
        public DateTime LastCheckedDateTimeUtc { get; set; } = DateTime.UtcNow;
    }
}