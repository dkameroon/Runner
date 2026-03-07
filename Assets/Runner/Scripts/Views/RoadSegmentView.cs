using UnityEngine;

public class RoadSegmentView : MonoBehaviour
{
    [field: SerializeField] public Transform StartPoint { get; private set; }

    [field: SerializeField] public Transform EndPoint { get; private set; }

    public Vector3 StartPosition => StartPoint.position;

    public Vector3 EndPosition => EndPoint.position;

    public Vector3 StartLocalPosition => StartPoint.localPosition;

    public Vector3 EndLocalPosition => EndPoint.localPosition;

    public RoadSegmentView SourcePrefab { get; private set; }

    public void SetSourcePrefab(RoadSegmentView sourcePrefab)
    {
        SourcePrefab = sourcePrefab;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, true);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void SetStartPosition(Vector3 worldStartPosition)
    {
        transform.position = worldStartPosition - StartLocalPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}