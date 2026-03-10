using System;
using System.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobRewardedAdsService : IAdsService, IDisposable
{
    private const string AndroidTestRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private const string IosTestRewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    private const string UnknownAdErrorMessage = "Unknown rewarded ad error.";

    private readonly AdMobAdsConfig _adMobAdsConfig;

    private RewardedAd _rewardedAd;
    private bool _isInitialized;
    private bool _isInitializing;
    private bool _isLoadingAd;

    public bool IsRewardedAdReady => _rewardedAd != null && _rewardedAd.CanShowAd();

    public AdMobRewardedAdsService(AdMobAdsConfig adMobAdsConfig)
    {
        _adMobAdsConfig = adMobAdsConfig;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        if (_isInitializing)
        {
            return;
        }

        _isInitializing = true;

        TaskCompletionSource<bool> initializeTaskCompletionSource = new();

        MobileAds.Initialize(initializationStatus =>
        {
            _isInitialized = initializationStatus != null;
            _isInitializing = false;

            if (_isInitialized)
            {
                initializeTaskCompletionSource.TrySetResult(true);
                return;
            }

            initializeTaskCompletionSource.TrySetResult(false);
        });

        bool isInitialized = await initializeTaskCompletionSource.Task;

        if (isInitialized == false)
        {
            Debug.LogError("AdMob initialization failed.");
            return;
        }

        await LoadRewardedAdAsync();
    }

    public async Task<RewardedAdResultData> ShowRewardedAdAsync()
    {
        await InitializeAsync();

        if (IsRewardedAdReady == false)
        {
            _ = LoadRewardedAdAsync();
            return RewardedAdResultData.NotReady();
        }

        TaskCompletionSource<RewardedAdResultData> showTaskCompletionSource = new();
        bool isRewardGranted = false;

        _rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            RewardedAdResultData result = isRewardGranted
                ? RewardedAdResultData.Success()
                : RewardedAdResultData.Cancelled();

            CleanupRewardedAd();
            _ = LoadRewardedAdAsync();

            showTaskCompletionSource.TrySetResult(result);
        };

        _rewardedAd.OnAdFullScreenContentFailed += adError =>
        {
            string errorMessage = adError != null
                ? adError.GetMessage()
                : UnknownAdErrorMessage;

            CleanupRewardedAd();
            _ = LoadRewardedAdAsync();

            showTaskCompletionSource.TrySetResult(RewardedAdResultData.Failure(errorMessage));
        };

        try
        {
            _rewardedAd.Show(reward =>
            {
                isRewardGranted = true;
            });
        }
        catch (Exception exception)
        {
            CleanupRewardedAd();
            _ = LoadRewardedAdAsync();

            return RewardedAdResultData.Failure(exception.Message);
        }

        return await showTaskCompletionSource.Task;
    }

    public void Dispose()
    {
        CleanupRewardedAd();
    }

    private async Task LoadRewardedAdAsync()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (_isLoadingAd)
        {
            return;
        }

        if (IsRewardedAdReady)
        {
            return;
        }

        _isLoadingAd = true;

        CleanupRewardedAd();

        TaskCompletionSource<bool> loadTaskCompletionSource = new();
        string adUnitId = GetRewardedAdUnitId();

        AdRequest adRequest = new();

        RewardedAd.Load(adUnitId, adRequest, (rewardedAd, loadAdError) =>
        {
            _isLoadingAd = false;

            if (loadAdError != null || rewardedAd == null)
            {
                string errorMessage = loadAdError != null
                    ? loadAdError.GetMessage()
                    : UnknownAdErrorMessage;

                Debug.LogWarning($"Rewarded ad load failed: {errorMessage}");
                loadTaskCompletionSource.TrySetResult(false);
                return;
            }

            _rewardedAd = rewardedAd;
            loadTaskCompletionSource.TrySetResult(true);
        });

        await loadTaskCompletionSource.Task;
    }

    private string GetRewardedAdUnitId()
    {
        if (_adMobAdsConfig.UseTestAdUnitIds)
        {
#if UNITY_IOS
            return IosTestRewardedAdUnitId;
#else
            return AndroidTestRewardedAdUnitId;
#endif
        }

#if UNITY_IOS
        return _adMobAdsConfig.IosRewardedAdUnitId;
#else
        return _adMobAdsConfig.AndroidRewardedAdUnitId;
#endif
    }

    private void CleanupRewardedAd()
    {
        if (_rewardedAd == null)
        {
            return;
        }

        _rewardedAd.Destroy();
        _rewardedAd = null;
    }
}