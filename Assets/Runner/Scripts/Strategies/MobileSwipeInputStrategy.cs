using System;
using UnityEngine;

public class MobileSwipeInputStrategy : IPlayerInputStrategy
{
    public event Action<EPlayerInputCommand> CommandTriggered;

    private const float MinSwipeDistanceScreenRatio = 0.06f;

    private Vector2 _startPosition;
    private int _activeFingerId = -1;
    private bool _isTracking;

    public void Tick()
    {
        if (Input.touchCount <= 0)
            return;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && !_isTracking)
            {
                _isTracking = true;
                _activeFingerId = touch.fingerId;
                _startPosition = touch.position;
                return;
            }

            if (!_isTracking || touch.fingerId != _activeFingerId)
                continue;

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                Vector2 delta = touch.position - _startPosition;

                float minDistance = Mathf.Min(Screen.width, Screen.height) * MinSwipeDistanceScreenRatio;

                if (delta.magnitude < minDistance)
                {
                    Reset();
                    return;
                }

                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    CommandTriggered?.Invoke(delta.x > 0f
                        ? EPlayerInputCommand.LaneRight
                        : EPlayerInputCommand.LaneLeft);
                }
                else
                {
                    CommandTriggered?.Invoke(delta.y > 0f
                        ? EPlayerInputCommand.Jump
                        : EPlayerInputCommand.Slide);
                }

                Reset();
                return;
            }
        }
    }

    private void Reset()
    {
        _isTracking = false;
        _activeFingerId = -1;
        _startPosition = Vector2.zero;
    }
}