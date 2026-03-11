using System.Threading.Tasks;

public interface IAuthenticationService
{
    bool IsAuthorized { get; }
    string UserId { get; }
    string UserEmail { get; }
    string UserLogin { get; }

    Task InitializeAsync();
    Task<bool> TrySilentSignInAsync();
    Task<AuthOperationResultData> SignInAsync(string email, string password);
    Task<AuthOperationResultData> SignUpAsync(string email, string login, string password);
    Task<AuthOperationResultData> SignOutAsync();
}