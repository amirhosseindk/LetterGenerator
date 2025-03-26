using LetterGenerator.Shared.Models;

namespace LetterGenerator.Letter.Models
{
    public class Letter : BaseEntity
    {
        public string Number { get; set; } = string.Empty;
        public string DatePersian { get; set; } = string.Empty;
        public DateTime DateUtc { get; set; } = DateTime.UtcNow;
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPosition { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderPosition { get; set; } = string.Empty;
        public bool HaveCopy { get; set; } = false;
        public string Copy { get; set; } = string.Empty;
    }
}