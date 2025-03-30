namespace LetterGenerator.Shared.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime InsertDateTimeUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDateTimeUtc { get; set; }
        public DateTime? DeletedDateTimeUtc { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}