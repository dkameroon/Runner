using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldSegmentPoolService : IWorldSegmentPoolService
{
    private readonly DiContainer _diContainer;
    private readonly SceneHierarchyService _sceneHierarchyService;

    private readonly Dictionary<RoadSegmentView, Queue<RoadSegmentView>> _poolByPrefab = new();

    private readonly Transform _poolRoot;

    public WorldSegmentPoolService(
        DiContainer diContainer,
        SceneHierarchyService sceneHierarchyService)
    {
        _diContainer = diContainer;
        _sceneHierarchyService = sceneHierarchyService;

        _poolRoot = _sceneHierarchyService.RoadSegmentsPoolRoot;
    }

    public RoadSegmentView Get(RoadSegmentView prefab)
    {
        if (!_poolByPrefab.TryGetValue(prefab, out Queue<RoadSegmentView> pool))
        {
            pool = new Queue<RoadSegmentView>();
            _poolByPrefab.Add(prefab, pool);
        }

        if (pool.Count > 0)
        {
            RoadSegmentView pooledSegment = pool.Dequeue();
            pooledSegment.Show();
            return pooledSegment;
        }

        RoadSegmentView createdSegment = _diContainer.InstantiatePrefabForComponent<RoadSegmentView>(prefab);
        createdSegment.SetSourcePrefab(prefab);
        createdSegment.Hide();

        return createdSegment;
    }

    public void Return(RoadSegmentView segment)
    {
        if (segment == null)
            return;

        RoadSegmentView sourcePrefab = segment.SourcePrefab;
        if (sourcePrefab == null)
        {
            Object.Destroy(segment.gameObject);
            return;
        }

        if (!_poolByPrefab.TryGetValue(sourcePrefab, out Queue<RoadSegmentView> pool))
        {
            pool = new Queue<RoadSegmentView>();
            _poolByPrefab.Add(sourcePrefab, pool);
        }

        segment.Hide();
        segment.SetParent(_poolRoot);

        pool.Enqueue(segment);
    }
}