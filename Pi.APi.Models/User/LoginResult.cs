namespace Pi.APi.Models.User
{
    public class LoginResult
    {
        public bool Success { get; private set; }
        public string Token { get; private set; }
        public string Error { get; private set; }
        public LoginErrorType ErrorType { get; private set; }

        private LoginResult(bool success, string token = null, LoginErrorType errorType = LoginErrorType.None, string error = null)
        {
            Success = success;
            Token = token;
            Error = error;
            ErrorType = errorType;
        }

        public static LoginResult Successful(string token) => new LoginResult(true, token);
        public static LoginResult Failed(LoginErrorType errorType, string error) => new LoginResult(false, errorType: errorType, error: error);
    }
}
