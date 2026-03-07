using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Configs/WorldGenerationConfig", fileName = "Config_WorldGeneration")]
public class WorldGenerationConfig : ScriptableObject
{
    [field: SerializeField] public List<RoadSegmentView> RoadSegmentPrefabs { get; private set; }

    [field: SerializeField] public int InitialSegmentsCount { get; private set; } = 6;

    [field: SerializeField] public float SegmentsAheadDistance { get; private set; } = 40f;

    [field: SerializeField] public float DespawnDistance { get; private set; } = 30f;
}