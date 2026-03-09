using System;
using Zenject;

public class DefeatContinueFlowService : IInitializable, ITickable, IDisposable
{
    private readonly PopupService _popupService;
    private readonly IAdsService _adsService;
    private readonly PlayerRespawnSystem _playerRespawnSystem;
    private readonly GameFlowSystem _gameFlowSystem;

    private DefeatPopup _subscribedPopup;
    private bool _isShowingRewardedAd;

    public DefeatContinueFlowService(
        PopupService popupService,
        IAdsService adsService,
        PlayerRespawnSystem playerRespawnSystem,
        GameFlowSystem gameFlowSystem)
    {
        _popupService = popupService;
        _adsService = adsService;
        _playerRespawnSystem = playerRespawnSystem;
        _gameFlowSystem = gameFlowSystem;
    }

    public void Initialize()
    {
    }

    public void Tick()
    {
        TrySubscribeToPopup();
    }

    public void Dispose()
    {
        UnsubscribeFromPopup();
    }

    private void TrySubscribeToPopup()
    {
        DefeatPopup currentPopup = _popupService.CurrentDefeatPopup;

        if (currentPopup == null)
        {
            return;
        }

        if (_subscribedPopup == currentPopup)
        {
            return;
        }

        UnsubscribeFromPopup();

        _subscribedPopup = currentPopup;
        _subscribedPopup.WatchAdClicked += OnWatchAdClicked;
    }

    private void UnsubscribeFromPopup()
    {
        if (_subscribedPopup == null)
        {
            return;
        }

        _subscribedPopup.WatchAdClicked -= OnWatchAdClicked;
        _subscribedPopup = null;
    }

    private async void OnWatchAdClicked()
    {
        if (_isShowingRewardedAd)
        {
            return;
        }

        if (_adsService.IsRewardedAdReady == false)
        {
            return;
        }

        _isShowingRewardedAd = true;

        RewardedAdResultData result = await _adsService.ShowRewardedAdAsync();

        _isShowingRewardedAd = false;

        if (result.IsSuccess == false || result.IsRewardGranted == false)
        {
            return;
        }

        _popupService.HideDefeatPopup();
        _playerRespawnSystem.ContinueAfterDefeat();
        _gameFlowSystem.ResumeGameAfterContinue();
    }
}