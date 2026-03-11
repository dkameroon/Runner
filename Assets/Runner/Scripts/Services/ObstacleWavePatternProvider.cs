using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWavePatternProvider
{
    private readonly ObstacleSpawnConfig _spawnConfig;
    private readonly ObstacleDifficultyProvider _obstacleDifficultyProvider;

    private readonly System.Random _random = new();
    private readonly List<ObstacleWavePatternStruct> _singleLanePatterns = new();
    private readonly List<ObstacleWavePatternStruct> _doubleLanePatterns = new();
    private readonly List<ObstacleWavePatternStruct> _tripleLanePatterns = new();

    public ObstacleWavePatternProvider(
        ObstacleSpawnConfig spawnConfig,
        ObstacleDifficultyProvider obstacleDifficultyProvider)
    {
        _spawnConfig = spawnConfig;
        _obstacleDifficultyProvider = obstacleDifficultyProvider;

        BuildWavePatternLibrary();
    }

    public ObstacleWavePatternStruct GetSmartRandomPattern(
        float activeGameplayTimeSeconds,
        int lastOccupiedLaneCount)
    {
        int occupiedLaneCount = GetSmartOccupiedLaneCount(
            activeGameplayTimeSeconds,
            lastOccupiedLaneCount);

        return occupiedLaneCount switch
        {
            1 => GetRandomPatternFromList(_singleLanePatterns),
            2 => GetRandomPatternFromList(_doubleLanePatterns),
            3 => GetRandomPatternFromList(_tripleLanePatterns),
            _ => GetRandomPatternFromList(_singleLanePatterns)
        };
    }

    private void BuildWavePatternLibrary()
    {
        _singleLanePatterns.Clear();
        _doubleLanePatterns.Clear();
        _tripleLanePatterns.Clear();

        EObstacleType[] allObstacleTypes =
        {
            EObstacleType.None,
            EObstacleType.Dodge,
            EObstacleType.Slide,
            EObstacleType.Jump
        };

        for (int i = 0; i < allObstacleTypes.Length; i++)
        {
            for (int j = 0; j < allObstacleTypes.Length; j++)
            {
                for (int k = 0; k < allObstacleTypes.Length; k++)
                {
                    ObstacleWavePatternStruct pattern = new ObstacleWavePatternStruct(
                        allObstacleTypes[i],
                        allObstacleTypes[j],
                        allObstacleTypes[k]);

                    if (IsPatternValid(pattern) == false)
                    {
                        continue;
                    }

                    switch (pattern.OccupiedLaneCount)
                    {
                        case 1:
                            _singleLanePatterns.Add(pattern);
                            break;

                        case 2:
                            _doubleLanePatterns.Add(pattern);
                            break;

                        case 3:
                            _tripleLanePatterns.Add(pattern);
                            break;
                    }
                }
            }
        }
    }

    private bool IsPatternValid(ObstacleWavePatternStruct pattern)
    {
        if (pattern.OccupiedLaneCount <= 0)
        {
            return false;
        }

        if (_spawnConfig.DisallowTripleDodgePattern && pattern.IsTripleDodgePattern)
        {
            return false;
        }

        return true;
    }

    private int GetSmartOccupiedLaneCount(
        float activeGameplayTimeSeconds,
        int lastOccupiedLaneCount)
    {
        _obstacleDifficultyProvider.GetPatternChances(
            activeGameplayTimeSeconds,
            out float singleChance,
            out float doubleChance,
            out float tripleChance);

        if (lastOccupiedLaneCount >= 2)
        {
            singleChance += 0.20f;
            doubleChance -= 0.10f;
            tripleChance -= 0.10f;
        }

        singleChance = Mathf.Max(0f, singleChance);
        doubleChance = Mathf.Max(0f, doubleChance);
        tripleChance = Mathf.Max(0f, tripleChance);

        float totalChance = singleChance + doubleChance + tripleChance;

        if (totalChance <= 0f)
        {
            return 1;
        }

        float roll = (float)_random.NextDouble() * totalChance;

        if (roll < singleChance)
        {
            return 1;
        }

        roll -= singleChance;

        if (roll < doubleChance)
        {
            return 2;
        }

        return 3;
    }

    private ObstacleWavePatternStruct GetRandomPatternFromList(List<ObstacleWavePatternStruct> patterns)
    {
        if (patterns.Count == 0)
        {
            return new ObstacleWavePatternStruct(
                EObstacleType.Dodge,
                EObstacleType.None,
                EObstacleType.None);
        }

        int index = _random.Next(0, patterns.Count);

        return patterns[index];
    }
}