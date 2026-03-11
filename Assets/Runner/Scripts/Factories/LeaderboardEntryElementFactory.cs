using Zenject;

public class LeaderboardEntryElementFactory
{
    private readonly LeaderboardEntryElement _leaderboardEntryElementPrefab;
    private readonly DiContainer _diContainer;

    public LeaderboardEntryElementFactory(
        LeaderboardEntryElement leaderboardEntryElementPrefab,
        DiContainer diContainer)
    {
        _leaderboardEntryElementPrefab = leaderboardEntryElementPrefab;
        _diContainer = diContainer;
    }

    public LeaderboardEntryElement Create()
    {
        return _diContainer.InstantiatePrefabForComponent<LeaderboardEntryElement>(
            _leaderboardEntryElementPrefab);
    }
}