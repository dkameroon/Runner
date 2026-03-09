using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MockLeaderboardService : ILeaderboardService
{
    private readonly List<LeaderboardEntryData> _entries = new();

    public MockLeaderboardService()
    {
        _entries.Add(new LeaderboardEntryData(1, "mock_user_1", "Alex", 1500));
        _entries.Add(new LeaderboardEntryData(2, "mock_user_2", "Viktor", 1200));
        _entries.Add(new LeaderboardEntryData(3, "mock_user_3", "John", 900));
        _entries.Add(new LeaderboardEntryData(4, "mock_user_4", "Kate", 700));
        _entries.Add(new LeaderboardEntryData(5, "mock_user_5", "Mike", 500));

        RebuildRanks();
    }

    public Task<IReadOnlyList<LeaderboardEntryData>> LoadTopEntriesAsync(int maxCount)
    {
        IReadOnlyList<LeaderboardEntryData> result = _entries
            .OrderByDescending(entry => entry.Score)
            .ThenBy(entry => entry.UserLogin)
            .Take(maxCount)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<LeaderboardSubmitResultData> SubmitScoreAsync(string userId, string userLogin, int score)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Task.FromResult(LeaderboardSubmitResultData.Failure("UserId is required."));
        }

        if (string.IsNullOrWhiteSpace(userLogin))
        {
            return Task.FromResult(LeaderboardSubmitResultData.Failure("UserLogin is required."));
        }

        LeaderboardEntryData existingEntry = _entries.FirstOrDefault(entry => entry.UserId == userId);

        if (existingEntry == null)
        {
            _entries.Add(new LeaderboardEntryData(0, userId, userLogin, score));
            RebuildRanks();
            return Task.FromResult(LeaderboardSubmitResultData.Success());
        }

        if (score > existingEntry.Score)
        {
            _entries.Remove(existingEntry);
            _entries.Add(new LeaderboardEntryData(0, userId, userLogin, score));
            RebuildRanks();
        }

        return Task.FromResult(LeaderboardSubmitResultData.Success());
    }

    private void RebuildRanks()
    {
        List<LeaderboardEntryData> sortedEntries = _entries
            .OrderByDescending(entry => entry.Score)
            .ThenBy(entry => entry.UserLogin)
            .ToList();

        _entries.Clear();

        for (int i = 0; i < sortedEntries.Count; i++)
        {
            LeaderboardEntryData entry = sortedEntries[i];

            _entries.Add(new LeaderboardEntryData(
                i + 1,
                entry.UserId,
                entry.UserLogin,
                entry.Score));
        }
    }
}