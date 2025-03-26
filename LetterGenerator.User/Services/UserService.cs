using LetterGenerator.Shared.Exceptions;
using LetterGenerator.Shared.Maps;
using LetterGenerator.Shared.Types;
using LetterGenerator.User.Contracts;
using LetterGenerator.User.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace LetterGenerator.User.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            _logger.LogInformation("Creating user {Username}", user.UserName);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("User creation failed: {Error}", error);

                throw new BusinessException(ErrorMap.GetMessage(ErrorType.UserCreationFailed), (int)ErrorType.UserCreationFailed);
            }

            await AddDefaultClaimsAsync(user);
            await AddDefaultRolesAsync(user);

            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser user)
        {
            _logger.LogInformation("Updating user {UserId}", user.Id);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("User update failed for {UserId}", user.Id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.UserUpdateFailed), (int)ErrorType.UserUpdateFailed);
            }
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user)
        {
            _logger.LogInformation("Deleting user {UserId}", user.Id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("User deletion failed for {UserId}", user.Id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.UserDeleteFailed), (int)ErrorType.UserDeleteFailed);
            }
            return result;
        }

        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.NullArgument), (int)ErrorType.NullArgument);

            _logger.LogInformation("Finding user by email: {Email}", email);
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser?> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.NullArgument), (int)ErrorType.NullArgument);

            _logger.LogInformation("Finding user by ID: {UserId}", userId);
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IList<Claim>> GetClaimsAsync(AppUser user)
        {
            if (user == null)
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.NullArgument), (int)ErrorType.NullArgument);

            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user)
        {
            if (user == null)
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.NullArgument), (int)ErrorType.NullArgument);

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                _logger.LogError("AddToRole failed for {UserId}", user.Id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.RoleAssignmentFailed), (int)ErrorType.RoleAssignmentFailed);
            }

            return result;
        }

        public async Task<IdentityResult> AddClaimAsync(AppUser user, Claim claim)
        {
            var result = await _userManager.AddClaimAsync(user, claim);
            if (!result.Succeeded)
            {
                _logger.LogError("AddClaim failed for {UserId}", user.Id);
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.ClaimAssignmentFailed), (int)ErrorType.ClaimAssignmentFailed);
            }

            return result;
        }

        public async Task<bool> SignInAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: true, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new BusinessException(ErrorMap.GetMessage(ErrorType.InvalidUsernameOrPassword), (int)ErrorType.InvalidUsernameOrPassword);
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await EnsureClaimsAsync(user);
                await EnsureRolesAsync(user);
            }

            return true;
        }

        public async Task<bool> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        private async Task AddDefaultRolesAsync(AppUser user)
        {
            string role = "Normal"; // پیش‌فرض
            await _userManager.AddToRoleAsync(user, role);
        }

        private async Task AddDefaultClaimsAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("FullName", user.FullName),
                new Claim("IsEnable", user.IsEnable.ToString())
            };

            foreach (var claim in claims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }
        }

        private async Task EnsureRolesAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Normal") && !roles.Contains("Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Normal");
            }
        }

        private async Task EnsureClaimsAsync(AppUser user)
        {
            var existingClaims = await _userManager.GetClaimsAsync(user);
            var claimsToAdd = new List<Claim>();

            if (!existingClaims.Any(c => c.Type == ClaimTypes.NameIdentifier))
                claimsToAdd.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            if (!existingClaims.Any(c => c.Type == ClaimTypes.Name))
                claimsToAdd.Add(new Claim(ClaimTypes.Name, user.UserName ?? ""));

            if (!existingClaims.Any(c => c.Type == ClaimTypes.Email))
                claimsToAdd.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));

            if (!existingClaims.Any(c => c.Type == "FullName"))
                claimsToAdd.Add(new Claim("FullName", user.FullName));

            if (!existingClaims.Any(c => c.Type == "IsEnable"))
                claimsToAdd.Add(new Claim("IsEnable", user.IsEnable.ToString()));

            foreach (var claim in claimsToAdd)
            {
                await _userManager.AddClaimAsync(user, claim);
            }
        }
    }
}