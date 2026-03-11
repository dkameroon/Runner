using Firebase;
using Firebase.Auth;

public class FirebaseAuthErrorMessageProvider
{
    public string GetMessage(FirebaseException firebaseException)
    {
        AuthError authError = (AuthError)firebaseException.ErrorCode;

        return authError switch
        {
            AuthError.InvalidEmail => "Invalid email address.",
            AuthError.MissingEmail => "Email is required.",
            AuthError.MissingPassword => "Password is required.",
            AuthError.WeakPassword => "Password is too weak.",
            AuthError.EmailAlreadyInUse => "This email is already in use.",
            AuthError.WrongPassword => "Incorrect password.",
            AuthError.UserNotFound => "User not found.",
            AuthError.NetworkRequestFailed => "Network error. Please try again.",
            AuthError.TooManyRequests => "Too many attempts. Please try again later.",
            _ => "Authentication error."
        };
    }
}