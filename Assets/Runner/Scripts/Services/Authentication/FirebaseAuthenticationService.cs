using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthenticationService : IAuthenticationService
{
    public bool IsAuthorized => CurrentUser != null;
    public string UserId => CurrentUser?.UserId ?? string.Empty;
    public string UserEmail => CurrentUser?.Email ?? string.Empty;
    public string UserLogin { get; private set; } = string.Empty;

    private readonly FirebaseAuthErrorMessageProvider _firebaseAuthErrorMessageProvider;
    private readonly FirebaseAuth _auth;

    private FirebaseUser CurrentUser => _auth.CurrentUser;

    public FirebaseAuthenticationService(
        FirebaseAuthErrorMessageProvider firebaseAuthErrorMessageProvider,
        FirebaseServiceProvider firebaseServiceProvider)
    {
        _firebaseAuthErrorMessageProvider = firebaseAuthErrorMessageProvider;
        _auth = firebaseServiceProvider.Auth;
    }

    public Task InitializeAsync()
    {
        if (CurrentUser != null)
        {
            UserLogin = CurrentUser.DisplayName ?? string.Empty;
        }

        return Task.CompletedTask;
    }

    public Task<bool> TrySilentSignInAsync()
    {
        bool isAuthorized = CurrentUser != null;

        if (isAuthorized)
        {
            UserLogin = CurrentUser.DisplayName ?? string.Empty;
        }

        return Task.FromResult(isAuthorized);
    }

    public async Task<AuthOperationResultData> SignInAsync(string email, string password)
    {
        try
        {
            AuthResult authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            UserLogin = user.DisplayName ?? string.Empty;

            return AuthOperationResultData.Success();
        }
        catch (FirebaseException firebaseException)
        {
            Debug.LogError(firebaseException);
            return AuthOperationResultData.Failure(
                _firebaseAuthErrorMessageProvider.GetMessage(firebaseException));
        }
    }

    public async Task<AuthOperationResultData> SignUpAsync(string email, string login, string password)
    {
        try
        {
            AuthResult authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            UserProfile userProfile = new UserProfile
            {
                DisplayName = login
            };

            await user.UpdateUserProfileAsync(userProfile);
            await user.ReloadAsync();

            UserLogin = login;

            return AuthOperationResultData.Success();
        }
        catch (FirebaseException firebaseException)
        {
            Debug.LogError(firebaseException);
            return AuthOperationResultData.Failure(
                _firebaseAuthErrorMessageProvider.GetMessage(firebaseException));
        }
    }

    public Task<AuthOperationResultData> SignOutAsync()
    {
        _auth.SignOut();
        UserLogin = string.Empty;

        return Task.FromResult(AuthOperationResultData.Success());
    }
}