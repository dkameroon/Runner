using UnityEngine;

public class ProjectBootstrapView : MonoBehaviour
{
    private SceneLoaderService _sceneLoaderService;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _sceneLoaderService = new SceneLoaderService();
        _sceneLoaderService.Load(SceneNames.Game);
    }
}