public class AuthOperationResultData
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    public AuthOperationResultData(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static AuthOperationResultData Success()
    {
        return new AuthOperationResultData(true, string.Empty);
    }

    public static AuthOperationResultData Failure(string errorMessage)
    {
        return new AuthOperationResultData(false, errorMessage);
    }
}