using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;

public class FirebaseLeaderboardService : ILeaderboardService
{
    private const string CollectionName = "leaderboard";
    private const string UserIdFieldName = "userId";
    private const string UserLoginFieldName = "userLogin";
    private const string ScoreFieldName = "score";

    private readonly FirebaseFirestore _firestore;

    public FirebaseLeaderboardService()
    {
        _firestore = FirebaseFirestore.DefaultInstance;
    }

    public async Task<IReadOnlyList<LeaderboardEntryData>> LoadTopEntriesAsync(int maxCount)
    {
        Query query = _firestore
            .Collection(CollectionName)
            .OrderByDescending(ScoreFieldName)
            .Limit(maxCount);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        List<LeaderboardEntryData> entries = new List<LeaderboardEntryData>();

        int rank = 1;

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            string userId = document.ContainsField(UserIdFieldName)
                ? document.GetValue<string>(UserIdFieldName)
                : document.Id;

            string userLogin = document.ContainsField(UserLoginFieldName)
                ? document.GetValue<string>(UserLoginFieldName)
                : "Player";

            int score = document.ContainsField(ScoreFieldName)
                ? document.GetValue<int>(ScoreFieldName)
                : 0;

            LeaderboardEntryData entryData = new LeaderboardEntryData(
                rank,
                userId,
                userLogin,
                score);

            entries.Add(entryData);
            rank++;
        }

        return entries;
    }

    public async Task<LeaderboardSubmitResultData> SubmitScoreAsync(string userId, string userLogin, int score)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return LeaderboardSubmitResultData.Failure("User id is empty.");
        }

        if (string.IsNullOrWhiteSpace(userLogin))
        {
            userLogin = "Player";
        }

        DocumentReference documentReference = _firestore
            .Collection(CollectionName)
            .Document(userId);

        DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();

        int currentBestScore = 0;

        if (snapshot.Exists && snapshot.ContainsField(ScoreFieldName))
        {
            currentBestScore = snapshot.GetValue<int>(ScoreFieldName);
        }

        if (score <= currentBestScore)
        {
            return LeaderboardSubmitResultData.Success();
        }

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { UserIdFieldName, userId },
            { UserLoginFieldName, userLogin },
            { ScoreFieldName, score }
        };

        await documentReference.SetAsync(data);

        return LeaderboardSubmitResultData.Success();
    }
}