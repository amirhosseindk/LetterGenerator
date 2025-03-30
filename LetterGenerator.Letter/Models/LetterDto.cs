namespace LetterGenerator.Letter.Models
{
    public class LetterDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime DateTimeLocal { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPosition { get; set; }
        public string Body { get; set; }
        public string SenderName { get; set; }
        public string SenderPosition { get; set; }
        public bool HaveCopy { get; set; }
        public string Copy { get; set; }
        public DateTime InsertDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
        public DateTime DeletedDateTimeUtc { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
    }
}