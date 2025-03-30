namespace LetterGenerator.Letter.Models
{
    public class CreateLetterDto
    {
        public string Number { get; set; }
        public DateTime DateTimeLocal { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPosition { get; set; }
        public string Body { get; set; }
        public string SenderName { get; set; }
        public string SenderPosition { get; set; }
        public bool HaveCopy { get; set; }
        public string Copy { get; set; }
        public string Username { get; set; }
    }
}