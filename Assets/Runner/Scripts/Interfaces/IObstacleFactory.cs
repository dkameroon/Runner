using UnityEngine;

public interface IObstacleFactory
{
    ObstacleView Create(EObstacleType obstacleType, Vector3 position, Quaternion rotation);
}