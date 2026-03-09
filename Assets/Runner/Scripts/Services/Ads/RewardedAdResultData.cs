public class RewardedAdResultData
{
    public bool IsSuccess { get; }
    public bool IsRewardGranted { get; }
    public string ErrorMessage { get; }

    public RewardedAdResultData(bool isSuccess, bool isRewardGranted, string errorMessage)
    {
        IsSuccess = isSuccess;
        IsRewardGranted = isRewardGranted;
        ErrorMessage = errorMessage;
    }

    public static RewardedAdResultData Success()
    {
        return new RewardedAdResultData(true, true, string.Empty);
    }

    public static RewardedAdResultData Failure(string errorMessage)
    {
        return new RewardedAdResultData(false, false, errorMessage);
    }

    public static RewardedAdResultData Cancelled()
    {
        return new RewardedAdResultData(false, false, "Rewarded ad was cancelled.");
    }

    public static RewardedAdResultData NotReady()
    {
        return new RewardedAdResultData(false, false, "Rewarded ad is not ready.");
    }
}