using System.Threading.Tasks;
using UnityEngine;

public class MockAdsService : IAdsService
{
    private const int MockRewardedDelayMilliseconds = 1500;

    public bool IsRewardedAdReady => true;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<RewardedAdResultData> ShowRewardedAdAsync()
    {
        await Task.Delay(MockRewardedDelayMilliseconds);
        return RewardedAdResultData.Success();
    }
}