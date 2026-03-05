public interface IPlayerState
{
    EPlayerState StateType { get; }
    void Enter();
    void Exit();
    void Tick();
}