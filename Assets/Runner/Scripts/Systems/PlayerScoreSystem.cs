using UnityEngine;

public class PlayerScoreSystem
{
    private float _distance;

    public int Score { get; private set; }

    public void UpdateScore(float deltaDistance)
    {
        if (deltaDistance <= 0f)
        {
            return;
        }

        _distance += deltaDistance;

        Score = Mathf.FloorToInt(_distance);
    }

    public void Reset()
    {
        _distance = 0f;
        Score = 0;
    }
}