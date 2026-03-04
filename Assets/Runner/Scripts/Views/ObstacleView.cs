using UnityEngine;

public class ObstacleView : MonoBehaviour
{
    [SerializeField] private EObstacleType _obstacleType;

    public EObstacleType ObstacleType => _obstacleType;
}