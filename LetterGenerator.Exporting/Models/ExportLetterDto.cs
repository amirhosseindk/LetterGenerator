namespace LetterGenerator.Exporting.Models
{
    public class ExportLetterDto
    {
        public string Number { get; set; } = string.Empty;
        public DateTime DateTimeLocal { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPosition { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderPosition { get; set; } = string.Empty;
        public bool HaveCopy { get; set; }
        public string Copy { get; set; } = string.Empty;
    }
}