public class PlayerModel
{
    public PlayerModel(EPlayerLane startLane)
    {
        CurrentLane = startLane;
    }

    public EPlayerLane CurrentLane { get; private set; }

    public void SetLane(EPlayerLane lane)
    {
        CurrentLane = lane;
    }
}