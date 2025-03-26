using LetterGenerator.Shared.Extentions;

namespace LetterGenerator.Shared.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string CreatedPersian { get; set; } = DateTime.UtcNow.ToPersian();

        public DateTime? ModifiedUtc { get; set; }
        public string ModifiedPersian { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
    }
}