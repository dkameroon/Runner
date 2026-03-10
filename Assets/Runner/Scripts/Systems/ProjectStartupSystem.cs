using UnityEngine;
using Zenject;

public class ProjectStartupSystem : IInitializable
{
    private readonly SceneLoaderService _sceneLoaderService;
    private readonly FirebaseBootstrapService _firebaseBootstrapService;
    private readonly AdsBootstrapService _adsBootstrapService;

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
        RunStartupAsync();
    }

    private async void RunStartupAsync()
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

        _sceneLoaderService.Load(SceneNames.Game);
    }
}