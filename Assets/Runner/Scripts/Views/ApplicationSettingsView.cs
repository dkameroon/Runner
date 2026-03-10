using UnityEngine;

public class ApplicationSettingsView : MonoBehaviour
{
    private const int TargetFrameRate = 60;

    private void Awake()
    {
        SetApplicationSettings();
    }

    private void SetApplicationSettings()
    {
        Application.targetFrameRate = TargetFrameRate;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        QualitySettings.vSyncCount = 0;
    }
}