public class LeaderboardEntryData
{
    public int Rank { get; }
    public string UserId { get; }
    public string UserLogin { get; }
    public int Score { get; }

    public LeaderboardEntryData(int rank, string userId, string userLogin, int score)
    {
        Rank = rank;
        UserId = userId;
        UserLogin = userLogin;
        Score = score;
    }
}