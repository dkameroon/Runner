public class GameplaySessionService
{
    public bool IsGameplayActive { get; private set; }

    public void Activate()
    {
        IsGameplayActive = true;
    }

    public void Deactivate()
    {
        IsGameplayActive = false;
    }
}