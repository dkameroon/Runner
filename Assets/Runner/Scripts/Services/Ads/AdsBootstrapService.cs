using System.Threading.Tasks;
using UnityEngine;

public class AdsBootstrapService
{
    private readonly IAdsService _adsService;
    private bool _isInitialized;
    private bool _isInitializing;

    public AdsBootstrapService(IAdsService adsService)
    {
        _adsService = adsService;
    }

    public async Task<bool> InitializeAsync()
    {
        if (_isInitialized)
        {
            return true;
        }

        if (_isInitializing)
        {
            return false;
        }

        _isInitializing = true;

        await _adsService.InitializeAsync();

        _isInitialized = true;
        _isInitializing = false;

        Debug.Log("Ads bootstrap initialization completed.");
        return true;
    }
}