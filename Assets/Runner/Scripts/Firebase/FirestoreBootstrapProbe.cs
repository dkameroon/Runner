using Firebase.Firestore;
using UnityEngine;
using Zenject;

public class FirestoreBootstrapProbe : IInitializable
{
    public void Initialize()
    {
        FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;
        Debug.Log("Firestore initialized successfully.");
    }
}