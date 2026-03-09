using System.Threading.Tasks;
using Firebase;
using UnityEngine;
using Zenject;

public class FirebaseBootstrapService : IInitializable
{
    public void Initialize()
    {
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var status = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (status != DependencyStatus.Available)
        {
            Debug.LogError($"Firebase dependencies error: {status}");
            return;
        }

        FirebaseApp app = FirebaseApp.DefaultInstance;

        Debug.Log("Firebase initialized successfully");
    }
}