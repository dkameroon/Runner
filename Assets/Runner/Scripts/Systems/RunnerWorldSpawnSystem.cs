using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RunnerWorldSpawnSystem : ITickable, IInitializable, IRestartable
{
    private readonly WorldGenerationConfig _worldGenerationConfig;
    private readonly PlayerView _playerView;
    private readonly IWorldSegmentPoolService _worldSegmentPoolService;
    private readonly SceneHierarchyService _sceneHierarchyService;

    private readonly List<RoadSegmentView> _activeSegments = new();
    public int ActiveSegmentCount => _activeSegments.Count;

    private readonly Transform _runtimeRoot;
    private Vector3 _nextSpawnPosition;

    public RunnerWorldSpawnSystem(
        WorldGenerationConfig worldGenerationConfig,
        PlayerView playerView,
        IWorldSegmentPoolService worldSegmentPoolService,
        SceneHierarchyService sceneHierarchyService)
    {
        _worldGenerationConfig = worldGenerationConfig;
        _playerView = playerView;
        _worldSegmentPoolService = worldSegmentPoolService;
        _sceneHierarchyService = sceneHierarchyService;

        _runtimeRoot = _sceneHierarchyService.RoadSegmentsRuntimeRoot;
    }

    public void Initialize()
    {
        CreateInitialSegments();
    }

    public void Tick()
    {
        if (_activeSegments.Count == 0)
            return;

        TrySpawnNextSegment();
        DespawnPassedSegments();
    }

    public void Restart()
    {
        ClearSegments();
        CreateInitialSegments();
    }

    private void CreateInitialSegments()
    {
        _activeSegments.Clear();

        RoadSegmentView prefab = GetRandomSegmentPrefab();

        float segmentLength = prefab.EndLocalPosition.z - prefab.StartLocalPosition.z;

        Vector3 playerPosition = _playerView.transform.position;

        Vector3 firstStartPosition = new Vector3(
            0f,
            0f,
            playerPosition.z - segmentLength);

        _nextSpawnPosition = firstStartPosition;

        int initialCount = Mathf.Max(1, _worldGenerationConfig.InitialSegmentsCount + 1);

        for (int i = 0; i < initialCount; i++)
        {
            SpawnNextSegment();
        }
    }

    private void TrySpawnNextSegment()
    {
        float playerZ = _playerView.transform.position.z;
        RoadSegmentView lastSegment = _activeSegments[_activeSegments.Count - 1];

        if (lastSegment.EndPosition.z - playerZ > _worldGenerationConfig.SegmentsAheadDistance)
            return;

        SpawnNextSegment();
    }

    private void SpawnNextSegment()
    {
        RoadSegmentView prefab = GetRandomSegmentPrefab();
        RoadSegmentView segment = _worldSegmentPoolService.Get(prefab);

        segment.SetParent(_runtimeRoot);
        segment.SetRotation(Quaternion.identity);
        segment.SetStartPosition(_nextSpawnPosition);
        segment.Show();

        _activeSegments.Add(segment);
        _nextSpawnPosition = segment.EndPosition;
    }

    private void DespawnPassedSegments()
    {
        float playerZ = _playerView.transform.position.z;

        for (int i = _activeSegments.Count - 1; i >= 0; i--)
        {
            RoadSegmentView segment = _activeSegments[i];

            if (playerZ - segment.EndPosition.z < _worldGenerationConfig.DespawnDistance)
                continue;

            _worldSegmentPoolService.Return(segment);
            _activeSegments.RemoveAt(i);
        }
    }

    private void ClearSegments()
    {
        for (int i = _activeSegments.Count - 1; i >= 0; i--)
        {
            RoadSegmentView segment = _activeSegments[i];

            if (segment == null)
                continue;

            _worldSegmentPoolService.Return(segment);
        }

        _activeSegments.Clear();
    }

    private RoadSegmentView GetRandomSegmentPrefab()
    {
        IReadOnlyList<RoadSegmentView> prefabs = _worldGenerationConfig.RoadSegmentPrefabs;

        int index = Random.Range(0, prefabs.Count);
        return prefabs[index];
    }
}