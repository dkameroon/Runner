public class LeaderboardSubmitResultData
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    public LeaderboardSubmitResultData(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static LeaderboardSubmitResultData Success()
    {
        return new LeaderboardSubmitResultData(true, string.Empty);
    }

    public static LeaderboardSubmitResultData Failure(string errorMessage)
    {
        return new LeaderboardSubmitResultData(false, errorMessage);
    }
}