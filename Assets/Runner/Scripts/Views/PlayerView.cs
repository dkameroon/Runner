using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public void SetPositionX(float x)
    {
        Vector3 position = transform.position;
        position.x = x;
        transform.position = position;
    }

    public void SetPositionY(float y)
    {
        Vector3 position = transform.position;
        position.y = y;
        transform.position = position;
    }
}