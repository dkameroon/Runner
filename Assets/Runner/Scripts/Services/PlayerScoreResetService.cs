public class PlayerScoreResetService
{
    private readonly PlayerScoreSystem _playerScoreSystem;
    private readonly PlayerScoreUpdateView _playerScoreUpdateView;

    public PlayerScoreResetService(
        PlayerScoreSystem playerScoreSystem,
        PlayerScoreUpdateView playerScoreUpdateView)
    {
        _playerScoreSystem = playerScoreSystem;
        _playerScoreUpdateView = playerScoreUpdateView;
    }

    public void ResetScore()
    {
        _playerScoreUpdateView.ResetTracking();
        _playerScoreSystem.Reset();
    }
}