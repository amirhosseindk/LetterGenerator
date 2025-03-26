using LetterGenerator.Shared.Extentions;
using Microsoft.AspNetCore.Identity;

namespace LetterGenerator.User.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsEnable { get; set; } = true;
        public DateTime RegisteredUtc { get; set; } = DateTime.UtcNow;
        public string RegisteredPersian { get; set; } = DateTime.UtcNow.ToPersian();
    }
}