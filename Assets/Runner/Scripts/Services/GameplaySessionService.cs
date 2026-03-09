public class GameplaySessionService
{
    public bool IsGameplayActive { get; private set; }

    public void StartGameplay()
    {
        IsGameplayActive = true;
    }

    public void StopGameplay()
    {
        IsGameplayActive = false;
    }

    public void SetGameplayActive(bool isActive)
    {
        IsGameplayActive = isActive;
    }
}