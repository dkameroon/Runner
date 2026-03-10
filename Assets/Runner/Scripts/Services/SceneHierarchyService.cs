using UnityEngine;

public class SceneHierarchyService
{
    public Transform RuntimeRoot { get; }
    public Transform PoolsRoot { get; }

    public Transform RoadSegmentsRuntimeRoot { get; }
    public Transform ObstaclesRuntimeRoot { get; }

    public Transform RoadSegmentsPoolRoot { get; }
    public Transform ObstaclesPoolRoot { get; }

    public SceneHierarchyService()
    {
        RuntimeRoot = CreateRoot("===Runtime===");
        PoolsRoot = CreateRoot("===Pools===");

        RoadSegmentsRuntimeRoot = CreateChildRoot("RoadSegments_RuntimeRoot", RuntimeRoot);
        ObstaclesRuntimeRoot = CreateChildRoot("Obstacles_RuntimeRoot", RuntimeRoot);

        RoadSegmentsPoolRoot = CreateChildRoot("RoadSegments_PoolRoot", PoolsRoot);
        ObstaclesPoolRoot = CreateChildRoot("Obstacles_PoolRoot", PoolsRoot);
    }

    private static Transform CreateRoot(string name)
    {
        GameObject rootObject = new GameObject(name);
        return rootObject.transform;
    }

    private static Transform CreateChildRoot(string name, Transform parent)
    {
        GameObject childObject = new GameObject(name);
        childObject.transform.SetParent(parent, false);
        return childObject.transform;
    }
}