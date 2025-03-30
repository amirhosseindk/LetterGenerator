namespace LetterGenerator.Letter.Models
{
    public class UpdateLetterDto : CreateLetterDto
    {
        public Guid Id { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
    }
}