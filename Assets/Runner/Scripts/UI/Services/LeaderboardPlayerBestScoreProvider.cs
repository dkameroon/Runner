using System.Collections.Generic;

public class LeaderboardPlayerBestScoreProvider
{
    private const string DefaultPlayerLogin = "Player";
    private const int DefaultBestScoreValue = 0;

    private readonly IAuthenticationService _authenticationService;

    public LeaderboardPlayerBestScoreProvider(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public string GetPlayerLogin()
    {
        if (string.IsNullOrWhiteSpace(_authenticationService.UserLogin))
        {
            return DefaultPlayerLogin;
        }

        return _authenticationService.UserLogin;
    }

    public int GetPlayerBestScore(IReadOnlyList<LeaderboardEntryData> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            return DefaultBestScoreValue;
        }

        string currentUserId = _authenticationService.UserId;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return DefaultBestScoreValue;
        }

        for (int index = 0; index < entries.Count; index++)
        {
            LeaderboardEntryData entry = entries[index];

            if (entry.UserId != currentUserId)
            {
                continue;
            }

            return entry.Score;
        }

        return DefaultBestScoreValue;
    }
}