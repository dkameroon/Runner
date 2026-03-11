public class ObstacleDifficultyProvider
{
    private readonly ObstacleDifficultyConfig _difficultyConfig;

    public ObstacleDifficultyProvider(ObstacleDifficultyConfig difficultyConfig)
    {
        _difficultyConfig = difficultyConfig;
    }

    public float GetSpawnIntervalSeconds(float activeGameplayTimeSeconds)
    {
        if (activeGameplayTimeSeconds < _difficultyConfig.MediumDifficultyTime)
        {
            return _difficultyConfig.EasySpawnIntervalSeconds;
        }

        if (activeGameplayTimeSeconds < _difficultyConfig.HardDifficultyTime)
        {
            return _difficultyConfig.MediumSpawnIntervalSeconds;
        }

        return _difficultyConfig.HardSpawnIntervalSeconds;
    }

    public void GetPatternChances(
        float activeGameplayTimeSeconds,
        out float singleChance,
        out float doubleChance,
        out float tripleChance)
    {
        if (activeGameplayTimeSeconds < _difficultyConfig.MediumDifficultyTime)
        {
            singleChance = _difficultyConfig.EasySingleLaneChance;
            doubleChance = _difficultyConfig.EasyDoubleLaneChance;
            tripleChance = _difficultyConfig.EasyTripleLaneChance;
            return;
        }

        if (activeGameplayTimeSeconds < _difficultyConfig.HardDifficultyTime)
        {
            singleChance = _difficultyConfig.MediumSingleLaneChance;
            doubleChance = _difficultyConfig.MediumDoubleLaneChance;
            tripleChance = _difficultyConfig.MediumTripleLaneChance;
            return;
        }

        singleChance = _difficultyConfig.HardSingleLaneChance;
        doubleChance = _difficultyConfig.HardDoubleLaneChance;
        tripleChance = _difficultyConfig.HardTripleLaneChance;
    }
}