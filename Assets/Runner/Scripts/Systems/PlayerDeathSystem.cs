public class PlayerDeathSystem
{
    public bool IsDead { get; private set; }

    public void Kill()
    {
        IsDead = true;
    }

    public void Reset()
    {
        IsDead = false;
    }
}