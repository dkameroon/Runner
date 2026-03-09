public class AuthenticationSessionData
{
    public string UserId { get; }
    public string UserEmail { get; }
    public string UserLogin { get; }

    public AuthenticationSessionData(string userId, string userEmail, string userLogin)
    {
        UserId = userId;
        UserEmail = userEmail;
        UserLogin = userLogin;
    }

    public static AuthenticationSessionData Empty()
    {
        return new AuthenticationSessionData(string.Empty, string.Empty, string.Empty);
    }
}