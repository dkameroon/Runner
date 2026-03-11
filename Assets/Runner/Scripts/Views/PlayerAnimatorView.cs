using UnityEngine;

public class PlayerAnimatorView : MonoBehaviour
{
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int SlideHash = Animator.StringToHash("Slide");

    [SerializeField] private Animator _animator;

    private float _defaultAnimatorSpeed = 1f;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        if (_animator == null)
        {
            Debug.LogError("PlayerAnimatorView: Animator not found in children.");
            return;
        }

        _defaultAnimatorSpeed = _animator.speed;
    }

    public void SetRunning(bool isRunning)
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetBool(IsRunningHash, isRunning);
    }

    public void SetDead(bool isDead)
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetBool(IsDeadHash, isDead);
    }

    public void TriggerJump()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetTrigger(JumpHash);
    }

    public void TriggerSlide()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.SetTrigger(SlideHash);
    }

    public void ResetTriggers()
    {
        if (_animator == null)
        {
            return;
        }

        _animator.ResetTrigger(JumpHash);
        _animator.ResetTrigger(SlideHash);
    }

    public void SetPaused(bool isPaused)
    {
        if (_animator == null)
        {
            return;
        }

        _animator.speed = isPaused ? 0f : _defaultAnimatorSpeed;
    }
}