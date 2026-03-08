using UnityEngine;
using Zenject;

public class ProjectStartupSystem : IInitializable
{
    private readonly SceneLoaderService _sceneLoaderService;

    public ProjectStartupSystem(SceneLoaderService sceneLoaderService)
    {
        _sceneLoaderService = sceneLoaderService;
    }

    public void Initialize()
    {
        _sceneLoaderService.Load(SceneNames.Game);
    }
}