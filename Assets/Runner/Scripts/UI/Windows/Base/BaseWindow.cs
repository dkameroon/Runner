using UnityEngine;

public abstract class BaseWindow : MonoBehaviour
{
    [SerializeField] protected GameObject _root;

    protected virtual void Awake()
    {
        Hide();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);

        if (_root != null)
        {
            _root.SetActive(true);
        }
    }

    public virtual void Hide()
    {
        if (_root != null)
        {
            _root.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}