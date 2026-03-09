using System.Collections.Generic;

public class ObstacleRegistryService
{
    private readonly List<ObstacleView> _active = new();

    public IReadOnlyList<ObstacleView> Active => _active;

    public void Add(ObstacleView obstacleView)
    {
        if (obstacleView == null)
            return;

        _active.Add(obstacleView);
    }

    public void RemoveAt(int index)
    {
        _active.RemoveAt(index);
    }

}