using System;
using System.Threading.Tasks;
using Zenject;

public class DefeatContinueFlowService : IInitializable, IDisposable
{
    private readonly GamePopupService _gamePopupService;
    private readonly IAdsService _adsService;
    private readonly PlayerContinueService _playerContinueService;
    private readonly GameFlowSystem _gameFlowSystem;
    private readonly PlayerRespawnSystem _playerRespawnSystem;

    private DefeatPopup _subscribedPopup;
    private bool _isShowingRewardedAd;

    public DefeatContinueFlowService(
        GamePopupService gamePopupService,
        IAdsService adsService,
        PlayerContinueService playerContinueService,
        GameFlowSystem gameFlowSystem,
        PlayerRespawnSystem playerRespawnSystem)
    {
        _gamePopupService = gamePopupService;
        _adsService = adsService;
        _playerContinueService = playerContinueService;
        _gameFlowSystem = gameFlowSystem;
        _playerRespawnSystem = playerRespawnSystem;
    }

    public void Initialize()
    {
        _gamePopupService.DefeatPopupShown += OnDefeatPopupShown;
        _gamePopupService.DefeatPopupHidden += OnDefeatPopupHidden;
    }

    public void Dispose()
    {
        _gamePopupService.DefeatPopupShown -= OnDefeatPopupShown;
        _gamePopupService.DefeatPopupHidden -= OnDefeatPopupHidden;

        UnsubscribeFromPopup();
    }

    private void OnDefeatPopupShown(DefeatPopup defeatPopup)
    {
        if (defeatPopup == null)
        {
            return;
        }

        if (_subscribedPopup == defeatPopup)
        {
            return;
        }

        UnsubscribeFromPopup();

        _subscribedPopup = defeatPopup;
        _subscribedPopup.RestartClicked += OnRestartClicked;
        _subscribedPopup.MainMenuClicked += OnMainMenuClicked;
        _subscribedPopup.WatchAdClicked += OnWatchAdClicked;
    }

    private void OnDefeatPopupHidden()
    {
        UnsubscribeFromPopup();
    }

    private void UnsubscribeFromPopup()
    {
        if (_subscribedPopup == null)
        {
            return;
        }

        _subscribedPopup.RestartClicked -= OnRestartClicked;
        _subscribedPopup.MainMenuClicked -= OnMainMenuClicked;
        _subscribedPopup.WatchAdClicked -= OnWatchAdClicked;
        _subscribedPopup = null;
    }

    private void OnRestartClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.StartGame();
    }

    private void OnMainMenuClicked()
    {
        _playerRespawnSystem.Respawn();
        _gameFlowSystem.EnterMainMenu();
    }

    private void OnWatchAdClicked()
    {
        _ = HandleWatchAdAsync();
    }

    private async Task HandleWatchAdAsync()
    {
        if (_isShowingRewardedAd)
        {
            return;
        }

        if (_adsService.IsRewardedAdReady == false)
        {
            if (_subscribedPopup != null)
            {
                _subscribedPopup.SetWatchAdButtonState(false);
            }

            return;
        }

        _isShowingRewardedAd = true;

        if (_subscribedPopup != null)
        {
            _subscribedPopup.SetWatchAdButtonState(false);
        }

        try
        {
            RewardedAdResultData result = await _adsService.ShowRewardedAdAsync();

            if (result.IsSuccess == false || result.IsRewardGranted == false)
            {
                if (_subscribedPopup != null)
                {
                    _subscribedPopup.SetWatchAdButtonState(_adsService.IsRewardedAdReady);
                }

                return;
            }

            _gamePopupService.HideDefeatPopup();
            _playerContinueService.ContinueAfterDefeat();
            _gameFlowSystem.ResumeGameAfterContinue();
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogException(exception);

            if (_subscribedPopup != null)
            {
                _subscribedPopup.SetWatchAdButtonState(_adsService.IsRewardedAdReady);
            }
        }
        finally
        {
            _isShowingRewardedAd = false;
        }
    }
}