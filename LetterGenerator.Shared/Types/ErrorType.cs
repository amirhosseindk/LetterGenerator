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
        UnauthorizedAccess = 2104,

        // Letter Errors
        LetterRepoNotFound = 3000,
        LetterRepoCreationFailed = 3001,
        LetterRepoUpdateFailed = 3002,
        LetterRepoGetAllFailed = 3003,
        LetterRepoDeleteFailed = 3004,
        LetterStatusRepoCreationFailed = 3005,
        LetterStatusRepoUpdateFailed = 3006,
        LetterStatusRepoGetAllFailed = 3007,
        LetterServNotFound = 3100,
        LetterServCreationFailed = 3101,
        LetterServUpdateFailed = 3102,
        LetterServGetAllFailed = 3103,
        LetterServDeleteFailed = 3104,
        LetterServSoftDeleteFailed = 3105,
        LetterServMapToLetterDtoFailed = 3106
    }
}