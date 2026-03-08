using System;
using UnityEngine;

public class EditorKeyboardInputStrategy : IPlayerInputStrategy
{
    public event Action<EPlayerInputCommand> CommandTriggered;

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CommandTriggered?.Invoke(EPlayerInputCommand.LaneLeft);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CommandTriggered?.Invoke(EPlayerInputCommand.LaneRight);
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CommandTriggered?.Invoke(EPlayerInputCommand.Jump);
            return;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            CommandTriggered?.Invoke(EPlayerInputCommand.Slide);
        }
    }
}