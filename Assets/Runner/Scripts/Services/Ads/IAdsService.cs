using System.Threading.Tasks;

public interface IAdsService
{
    bool IsRewardedAdReady { get; }

    Task InitializeAsync();
    Task<RewardedAdResultData> ShowRewardedAdAsync();
}