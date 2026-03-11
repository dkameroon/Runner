using Firebase.Auth;
using Firebase.Firestore;

public class FirebaseServiceProvider
{
    public FirebaseAuth Auth => FirebaseAuth.DefaultInstance;
    public FirebaseFirestore Firestore => FirebaseFirestore.DefaultInstance;
}