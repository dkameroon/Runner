using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILeaderboardService
{
    Task<IReadOnlyList<LeaderboardEntryData>> LoadTopEntriesAsync(int maxCount);
    Task<LeaderboardSubmitResultData> SubmitScoreAsync(string userId, string userLogin, int score);
}