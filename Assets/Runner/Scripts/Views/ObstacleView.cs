using UnityEngine;

public class ObstacleView : MonoBehaviour
{
    [field: SerializeField] public EObstacleType ObstacleType { get; private set; }

    [SerializeField] private ObstacleSpawnPointHolder _spawnPointHolder;

    private const string ObstacleDeathLayerName = "ObstacleDeath";

    private void Awake()
    {
        ApplyDeathLayerToHierarchy();
    }

    private void OnEnable()
    {
        ApplyDeathLayerToHierarchy();
    }

    public void SetPosition(Vector3 position)
    {
        if (_spawnPointHolder == null || _spawnPointHolder.SpawnPoint == null)
        {
            transform.position = position;
            return;
        }

        Vector3 offset = transform.position - _spawnPointHolder.SpawnPoint.position;
        transform.position = position + offset;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ApplyDeathLayerToHierarchy()
    {
        int layer = LayerMask.NameToLayer(ObstacleDeathLayerName);
        if (layer < 0)
            return;

        SetLayerRecursive(transform, layer);
    }

    private static void SetLayerRecursive(Transform root, int layer)
    {
        root.gameObject.layer = layer;

        for (int i = 0; i < root.childCount; i++)
        {
            SetLayerRecursive(root.GetChild(i), layer);
        }
    }
}