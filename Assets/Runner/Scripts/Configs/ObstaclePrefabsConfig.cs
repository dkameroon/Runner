using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ObstaclePrefabsConfig",
    menuName = "Runner/Configs/ObstaclePrefabsConfig")]
public class ObstaclePrefabsConfig : ScriptableObject
{
    [SerializeField] private List<ObstaclePrefabGroupStruct> _groups = new();

    public IReadOnlyList<ObstaclePrefabGroupStruct> Groups => _groups;

    public bool TryGetGroup(EObstacleType obstacleType, out ObstaclePrefabGroupStruct group)
    {
        if (obstacleType == EObstacleType.None)
        {
            group = default;
            return false;
        }

        for (int i = 0; i < _groups.Count; i++)
        {
            if (_groups[i].ObstacleType != obstacleType)
            {
                continue;
            }

            group = _groups[i];
            return true;
        }

        group = default;
        return false;
    }
}

[Serializable]
public struct ObstaclePrefabGroupStruct
{
    [field: SerializeField] public EObstacleType ObstacleType { get; private set; }
    [field: SerializeField] public List<ObstacleView> Prefabs { get; private set; }
}