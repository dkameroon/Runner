using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ProjectStartupSystem : IInitializable
{
    private readonly SceneLoaderService _sceneLoaderService;
    private readonly FirebaseBootstrapService _firebaseBootstrapService;
    private readonly AdsBootstrapService _adsBootstrapService;

    private bool _isStartupRunning;
    private bool _isStartupCompleted;

    public ProjectStartupSystem(
        SceneLoaderService sceneLoaderService,
        FirebaseBootstrapService firebaseBootstrapService,
        AdsBootstrapService adsBootstrapService)
    {
        _sceneLoaderService = sceneLoaderService;
        _firebaseBootstrapService = firebaseBootstrapService;
        _adsBootstrapService = adsBootstrapService;
    }

    public void Initialize()
    {
        StartStartupFlow();
    }

    private void StartStartupFlow()
    {
        if (_isStartupRunning || _isStartupCompleted)
        {
            return;
        }

        _ = RunStartupAsync();
    }

    private async Task RunStartupAsync()
    {
        if (_isStartupRunning || _isStartupCompleted)
        {
            return;
        }

        _isStartupRunning = true;

        try
        {
            bool firebaseInitialized = await _firebaseBootstrapService.InitializeAsync();

            if (firebaseInitialized == false)
            {
                Debug.LogError("Project startup aborted: Firebase initialization failed.");
                return;
            }

            bool adsInitialized = await _adsBootstrapService.InitializeAsync();

            if (adsInitialized == false)
            {
                Debug.LogError("Project startup aborted: Ads initialization failed.");
                return;
            }

            _isStartupCompleted = true;
            _sceneLoaderService.Load(SceneNames.Game);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
        finally
        {
            _isStartupRunning = false;
        }
    }
}