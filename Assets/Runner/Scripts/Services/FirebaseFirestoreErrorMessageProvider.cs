using Firebase;

public class FirebaseFirestoreErrorMessageProvider
{
    public string GetMessage(FirebaseException firebaseException)
    {
        return "Leaderboard request failed. Please try again.";
    }
}