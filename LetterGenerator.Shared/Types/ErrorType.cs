namespace LetterGenerator.Shared.Types
{
    public enum ErrorType
    {
        // General
        NullArgument = 1000,
        InternalServerError = 1001,

        // User
        UserNotFound = 2000,
        UserCreationFailed = 2001,
        UserUpdateFailed = 2002,
        UserDeleteFailed = 2003,
        InvalidUsernameOrPassword = 2004,
        InvalidPhoneNumber = 2005,
        ExistingPhoneNumber = 2006,
        ExistingEmailAddress = 2007,

        // Role / Claims
        RoleAssignmentFailed = 2100,
        ClaimAssignmentFailed = 2101,
        PasswordChangeFailed = 2102,
        EmailChangeFailed = 2103,
        UnauthorizedAccess = 2104
    }
}