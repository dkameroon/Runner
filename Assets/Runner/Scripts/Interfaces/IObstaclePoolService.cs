public interface IObstaclePoolService
{
    ObstacleView Get(EObstacleType obstacleType);
    void Return(ObstacleView obstacleView);
}