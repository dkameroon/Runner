using UnityEngine.SceneManagement;

public class SceneLoaderService
{
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}