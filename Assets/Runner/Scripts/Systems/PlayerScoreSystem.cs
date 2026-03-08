using System;
using UnityEngine;

public class PlayerScoreSystem
{
    public int CurrentScore { get; private set; }
    public float DistanceMeters { get; private set; }

    public event Action<int> ScoreChanged;

    private readonly RunnerGameConfig _runnerGameConfig;

    public PlayerScoreSystem(RunnerGameConfig runnerGameConfig)
    {
        _runnerGameConfig = runnerGameConfig;
        Reset();
    }

    public void Reset()
    {
        CurrentScore = 0;
        DistanceMeters = 0f;
        ScoreChanged?.Invoke(CurrentScore);
    }

    public void AddDistance(float deltaMeters)
    {
        if (deltaMeters <= 0f)
            return;

        DistanceMeters += deltaMeters;

        int newScore = Mathf.FloorToInt(DistanceMeters * _runnerGameConfig.ScorePerMeter);
        if (newScore == CurrentScore)
            return;

        CurrentScore = newScore;
        ScoreChanged?.Invoke(CurrentScore);
    }
}