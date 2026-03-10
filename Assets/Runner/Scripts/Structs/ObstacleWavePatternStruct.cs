using System;

[Serializable]
public struct ObstacleWavePatternStruct
{
    public EObstacleType LeftObstacleType;
    public EObstacleType CenterObstacleType;
    public EObstacleType RightObstacleType;

    public ObstacleWavePatternStruct(
        EObstacleType leftObstacleType,
        EObstacleType centerObstacleType,
        EObstacleType rightObstacleType)
    {
        LeftObstacleType = leftObstacleType;
        CenterObstacleType = centerObstacleType;
        RightObstacleType = rightObstacleType;
    }

    public bool HasLeftLane => LeftObstacleType != EObstacleType.None;
    public bool HasCenterLane => CenterObstacleType != EObstacleType.None;
    public bool HasRightLane => RightObstacleType != EObstacleType.None;

    public int OccupiedLaneCount
    {
        get
        {
            int count = 0;

            if (HasLeftLane)
            {
                count++;
            }

            if (HasCenterLane)
            {
                count++;
            }

            if (HasRightLane)
            {
                count++;
            }

            return count;
        }
    }

    public bool IsTripleDodgePattern =>
        LeftObstacleType == EObstacleType.Dodge &&
        CenterObstacleType == EObstacleType.Dodge &&
        RightObstacleType == EObstacleType.Dodge;
}