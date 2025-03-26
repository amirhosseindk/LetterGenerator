using LetterGenerator.User.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LetterGenerator.User.Contracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<IdentityResult> UpdateUserAsync(AppUser user);
        Task<IdentityResult> DeleteAsync(AppUser user);
        Task<AppUser?> FindByEmailAsync(string email);
        Task<AppUser?> FindByIdAsync(string userId);
        Task<IList<Claim>> GetClaimsAsync(AppUser user);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<IdentityResult> AddToRoleAsync(AppUser user, string role);
        Task<IdentityResult> AddClaimAsync(AppUser user, Claim claim);
        Task<bool> SignInAsync(string userName, string password);
        Task<bool> SignOutAsync();
    }
}