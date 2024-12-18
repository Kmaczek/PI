namespace Pi.APi.Models.User
{
    public enum LoginErrorType
    {
        None,
        UserNotFound,
        InvalidPassword,
        AccountLocked,
        InvalidInput
    }
}
