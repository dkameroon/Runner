using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseLeaderboardService : ILeaderboardService
{
    private const string DefaultUserLogin = "Player";
    private const int DefaultScoreValue = 0;
    private const int InitialRankValue = 1;

    private readonly FirebaseFirestore _firestore;
    private readonly FirebaseFirestoreErrorMessageProvider _firebaseFirestoreErrorMessageProvider;

    public FirebaseLeaderboardService(
        FirebaseServiceProvider firebaseServiceProvider,
        FirebaseFirestoreErrorMessageProvider firebaseFirestoreErrorMessageProvider)
    {
        _firestore = firebaseServiceProvider.Firestore;
        _firebaseFirestoreErrorMessageProvider = firebaseFirestoreErrorMessageProvider;
    }

    public async Task<IReadOnlyList<LeaderboardEntryData>> LoadTopEntriesAsync(int maxCount)
    {
        try
        {
            Query query = _firestore
                .Collection(FirestoreCollectionNames.Leaderboard)
                .OrderByDescending(FirestoreFieldNames.Score)
                .Limit(maxCount);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            List<LeaderboardEntryData> entries = new List<LeaderboardEntryData>();

            int rank = InitialRankValue;

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                string userId = document.ContainsField(FirestoreFieldNames.UserId)
                    ? document.GetValue<string>(FirestoreFieldNames.UserId)
                    : document.Id;

                string userLogin = document.ContainsField(FirestoreFieldNames.UserLogin)
                    ? document.GetValue<string>(FirestoreFieldNames.UserLogin)
                    : DefaultUserLogin;

                int score = document.ContainsField(FirestoreFieldNames.Score)
                    ? document.GetValue<int>(FirestoreFieldNames.Score)
                    : DefaultScoreValue;

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
        catch (FirebaseException firebaseException)
        {
            Debug.LogError(_firebaseFirestoreErrorMessageProvider.GetMessage(firebaseException));
            Debug.LogException(firebaseException);

            return new List<LeaderboardEntryData>();
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception);
            return new List<LeaderboardEntryData>();
        }
    }

    public async Task<LeaderboardSubmitResultData> SubmitScoreAsync(string userId, string userLogin, int score)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return LeaderboardSubmitResultData.Failure("User id is empty.");
        }

        if (string.IsNullOrWhiteSpace(userLogin))
        {
            userLogin = DefaultUserLogin;
        }

        try
        {
            DocumentReference documentReference = _firestore
                .Collection(FirestoreCollectionNames.Leaderboard)
                .Document(userId);

            DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();

            int currentBestScore = DefaultScoreValue;

            if (snapshot.Exists && snapshot.ContainsField(FirestoreFieldNames.Score))
            {
                currentBestScore = snapshot.GetValue<int>(FirestoreFieldNames.Score);
            }

            if (score <= currentBestScore)
            {
                return LeaderboardSubmitResultData.Success();
            }

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { FirestoreFieldNames.UserId, userId },
                { FirestoreFieldNames.UserLogin, userLogin },
                { FirestoreFieldNames.Score, score }
            };

            await documentReference.SetAsync(data);

            return LeaderboardSubmitResultData.Success();
        }
        catch (FirebaseException firebaseException)
        {
            Debug.LogError(_firebaseFirestoreErrorMessageProvider.GetMessage(firebaseException));
            Debug.LogException(firebaseException);

            return LeaderboardSubmitResultData.Failure(
                _firebaseFirestoreErrorMessageProvider.GetMessage(firebaseException));
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception);
            return LeaderboardSubmitResultData.Failure("Unexpected leaderboard error.");
        }
    }
}