using UnityEngine;

[CreateAssetMenu( fileName = "DebugOverlayConfig", menuName = "Runner/Configs/DebugOverlayConfig")]
public class DebugOverlayConfig : ScriptableObject
{
    [field: SerializeField] public bool IsEnabled { get; private set; } = true;
    [field: SerializeField] public float RefreshInterval { get; private set; } = 0.25f;
    [field: SerializeField] public bool ShowOnlyInDevelopmentBuild { get; private set; } = true;
}