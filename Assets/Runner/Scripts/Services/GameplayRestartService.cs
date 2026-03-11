using System.Collections.Generic;

public class GameplayRestartService
{
    private readonly List<IRestartable> _restartables;

    public GameplayRestartService(List<IRestartable> restartables)
    {
        _restartables = restartables;
    }

    public void RestartAll()
    {
        for (int i = 0; i < _restartables.Count; i++)
        {
            _restartables[i].Restart();
        }
    }
}