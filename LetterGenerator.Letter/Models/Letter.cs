using LetterGenerator.Shared.Models;

namespace LetterGenerator.Letter.Models
{
    public class Letter : BaseEntity
    {
        public string Number { get; set; } = string.Empty;
        public DateTime DateTimeLocal { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientPosition { get; set; }
        public string Body { get; set; } = string.Empty;
        public string? SenderName { get; set; }
        public string? SenderPosition { get; set; }
        public bool HaveCopy { get; set; } = false;
        public string? Copy { get; set; }
    }
}