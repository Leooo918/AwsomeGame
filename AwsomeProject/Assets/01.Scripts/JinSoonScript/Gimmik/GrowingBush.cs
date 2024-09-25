using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrowingBush : MonoBehaviour, IAffectable
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;

    [SerializeField]
    private LayerMask _objLayer;
    [SerializeField]
    private Sprite _3x3Sprite;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }


    public void ApplyEffect()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");
        _spriteRenderer.sprite = _3x3Sprite;
        _collider.size = Vector2.one * 3;
    }
}
