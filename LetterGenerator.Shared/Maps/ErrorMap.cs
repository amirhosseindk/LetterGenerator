using LetterGenerator.Shared.Types;

namespace LetterGenerator.Shared.Maps
{
    public static class ErrorMap
    {
        private static readonly Dictionary<ErrorType, string> _errorMessages = new()
        {
            // General
            { ErrorType.NullArgument, "Argument should not be null or empty" },
            { ErrorType.InternalServerError, "An internal server error occurred" },

            // User
            { ErrorType.UserNotFound, "User not found" },
            { ErrorType.UserCreationFailed, "User creation failed" },
            { ErrorType.UserUpdateFailed, "User update failed" },
            { ErrorType.UserDeleteFailed, "User deletion failed" },
            { ErrorType.InvalidUsernameOrPassword, "Invalid username or password" },
            { ErrorType.InvalidPhoneNumber, "Invalid phone number" },
            { ErrorType.ExistingPhoneNumber, "Phone number already exists" },
            { ErrorType.ExistingEmailAddress, "Email address already exists" },

            // Role / Claims
            { ErrorType.RoleAssignmentFailed, "Failed to assign role to user" },
            { ErrorType.ClaimAssignmentFailed, "Failed to assign claim to user" },
            { ErrorType.PasswordChangeFailed, "Password change failed" },
            { ErrorType.EmailChangeFailed, "Email change failed" },
            { ErrorType.UnauthorizedAccess, "You are not authorized to perform this action" }
        };

        public static string GetMessage(ErrorType errorType) =>
            _errorMessages.TryGetValue(errorType, out var message) ? message : "Unknown error";
    }
}