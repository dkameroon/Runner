// using System;
// using System.Threading.Tasks;
//
// public class MockAuthenticationService : IAuthenticationService
// {
//     private AuthenticationSessionData _sessionData;
//
//     public bool IsAuthorized => string.IsNullOrWhiteSpace(_sessionData.UserId) == false;
//     public string UserId => _sessionData.UserId;
//     public string UserEmail => _sessionData.UserEmail;
//     public string UserLogin => _sessionData.UserLogin;
//
//     public MockAuthenticationService()
//     {
//         _sessionData = AuthenticationSessionData.Empty();
//     }
//
//     public Task InitializeAsync()
//     {
//         return Task.CompletedTask;
//     }
//
//     public Task<AuthOperationResultData> SignInAsync(string email, string password)
//     {
//         if (string.IsNullOrWhiteSpace(email))
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Email is required."));
//         }
//
//         if (string.IsNullOrWhiteSpace(password))
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Password is required."));
//         }
//
//         _sessionData = new AuthenticationSessionData(
//             Guid.NewGuid().ToString(),
//             email,
//             ExtractLoginFromEmail(email));
//
//         return Task.FromResult(AuthOperationResultData.Success());
//     }
//
//     public Task<AuthOperationResultData> SignUpAsync(string email, string login, string password, string confirmPassword)
//     {
//         if (string.IsNullOrWhiteSpace(email))
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Email is required."));
//         }
//
//         if (string.IsNullOrWhiteSpace(login))
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Login is required."));
//         }
//
//         if (string.IsNullOrWhiteSpace(password))
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Password is required."));
//         }
//
//         if (string.IsNullOrWhiteSpace(confirmPassword))
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Confirm password is required."));
//         }
//
//         if (password != confirmPassword)
//         {
//             return Task.FromResult(AuthOperationResultData.Failure("Passwords do not match."));
//         }
//
//         _sessionData = new AuthenticationSessionData(
//             Guid.NewGuid().ToString(),
//             email,
//             login);
//
//         return Task.FromResult(AuthOperationResultData.Success());
//     }
//
//     public Task<AuthOperationResultData> SignOutAsync()
//     {
//         _sessionData = AuthenticationSessionData.Empty();
//         return Task.FromResult(AuthOperationResultData.Success());
//     }
//
//     private static string ExtractLoginFromEmail(string email)
//     {
//         int separatorIndex = email.IndexOf('@');
//
//         if (separatorIndex <= 0)
//         {
//             return email;
//         }
//
//         return email.Substring(0, separatorIndex);
//     }
// }