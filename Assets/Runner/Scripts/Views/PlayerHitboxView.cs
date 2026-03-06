using UnityEngine;

public class PlayerHitboxView : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _capsuleCollider;

    [Header("Default")]
    [SerializeField] private float _defaultHeight = 2.0f;
    [SerializeField] private Vector3 _defaultCenter = new(0f, 1f, 0f);

    [Header("Slide")]
    [SerializeField] private float _slideHeight = 1.0f;
    [SerializeField] private Vector3 _slideCenter = new(0f, 0.5f, 0f);

    private const float MinHeight = 0.1f;

    private void Awake()
    {
        if (_capsuleCollider == null)
            _capsuleCollider = GetComponent<CapsuleCollider>();

        ApplyDefault();
    }

    public void ApplyDefault()
    {
        Apply(_defaultHeight, _defaultCenter);
    }

    public void ApplySlide()
    {
        Apply(_slideHeight, _slideCenter);
    }

    private void Apply(float height, Vector3 center)
    {
        if (_capsuleCollider == null)
            return;

        _capsuleCollider.height = Mathf.Max(height, MinHeight);
        _capsuleCollider.center = center;
    }
}