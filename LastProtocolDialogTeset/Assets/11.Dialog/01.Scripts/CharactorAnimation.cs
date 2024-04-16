using UnityEngine;
using UnityEngine.UI;

public abstract class CharactorAnimation : MonoBehaviour
{
    protected RectTransform rect;
    public bool isAnimating { get; protected set; }

    private void Awake()
    {
        isAnimating = false;
    }

    public abstract void Animation();

    public virtual void Init(RectTransform rect)
    {
        this.rect = rect;
    }
}
