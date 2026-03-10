using System.Threading.Tasks;
using Firebase;
using UnityEngine;

public class FirebaseBootstrapService
{
    private bool _isInitialized;
    private bool _isInitializing;

    public async Task<bool> InitializeAsync()
    {
        if (_isInitialized)
        {
            return true;
        }

        if (_isInitializing)
        {
            return false;
        }

        _isInitializing = true;

        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (status != DependencyStatus.Available)
        {
            _isInitializing = false;
            Debug.LogError($"Firebase dependencies error: {status}");
            return false;
        }

        FirebaseApp app = FirebaseApp.DefaultInstance;

        _isInitialized = true;
        _isInitializing = false;

        Debug.Log("Firebase initialized successfully");
        return true;
    }
}