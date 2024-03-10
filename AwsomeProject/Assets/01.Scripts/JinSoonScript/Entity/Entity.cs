using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region ComponentRegion
    public Animator animatorCompo { get; protected set; }
    public SpriteRenderer spriteRendererCompo { get; protected set; }
    public Collider2D colliderCompo { get; protected set; }
    public Rigidbody2D rigidbodyCompo { get; protected set; }
    #endregion

    [Header("Collision info")]
    [SerializeField] protected Transform groundChecker;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGroundAndWall;
    [SerializeField] protected LayerMask whatIsProbs;
    [SerializeField] protected Transform wallChecker;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float groundCheckBoxWidth;
    [SerializeField] protected float wallCheckBoxHeight;



    public Action<int> OnFlip;
    public int FacingDir { get; protected set; } = 1;
    public bool CanStateChangeable { get; set; } = true;


    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        animatorCompo = visualTrm.GetComponent<Animator>();
        spriteRendererCompo = visualTrm.GetComponent<SpriteRenderer>();
        rigidbodyCompo = GetComponent<Rigidbody2D>();
        colliderCompo = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }


    #region Velocity Section

    public void SetVelocity(float x, float y, bool doNotFlip = false)
    {
        rigidbodyCompo.velocity = new Vector2(x, y);
        if (!doNotFlip)
            FlipController(x);
    }

    /// <summary>
    /// Set velocity zero this Entity
    /// </summary>
    /// <param name="withYAxis">Stop With YAxis</param>
    public void StopImmediately(bool withYAxis)
    {
        if (withYAxis)
            rigidbodyCompo.velocity = Vector2.zero;
        else
            rigidbodyCompo.velocity = new Vector2(0, rigidbodyCompo.velocity.y);
    }

    #endregion


    #region FlipSection

    public virtual void Flip()
    {
        FacingDir = FacingDir * -1;
        transform.Rotate(0, 180f, 0);
        OnFlip?.Invoke(FacingDir);
    }

    public virtual void FlipController(float x)
    {
        if (Mathf.Abs(x) < 0.05f) return;
        x = Mathf.Sign(x); //x �� ��ȣ�� �������ŵ�
        if (Mathf.Abs(FacingDir + x) < 0.5f)
            Flip();
    }

    #endregion


    #region CheckCollisionSection

    public virtual bool IsGroundDetected() =>
         Physics2D.BoxCast(groundChecker.position,
            new Vector2(groundCheckBoxWidth, 0.05f), 0,
            Vector2.down, groundCheckDistance, whatIsGroundAndWall);


    public virtual bool IsWallDetected() =>
        Physics2D.BoxCast(wallChecker.position,
            new Vector2(0.05f, wallCheckBoxHeight), 0,
            Vector2.right * FacingDir, wallCheckDistance, whatIsGroundAndWall);

    public virtual void CheckObjectOnFoot()
    {
        RaycastHit2D hit = Physics2D.BoxCast(groundChecker.position,
            new Vector2(groundCheckBoxWidth, 0.05f), 0,
            Vector2.down, groundCheckDistance, whatIsProbs);

        if(hit.collider != null && hit.collider.TryGetComponent<Probs>(out Probs p))
        {
            p.Interact(this);
        }
    }

    #endregion


    public abstract void Attack();


    //�갡 �� ���� �� ���� ��Ű�� �׷��� �� ���� ���� ����
    public Coroutine StartDelayCallBack(float delay, Action callBack)
    {
        return StartCoroutine(DelayCoroutine(delay, callBack));
    }

    IEnumerator DelayCoroutine(float delay, Action callBack)
    {
        yield return new WaitForSeconds(delay);
        callBack?.Invoke();
    }

#if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(wallChecker.position, 
            new Vector2(wallCheckDistance, wallCheckBoxHeight));
        Gizmos.DrawWireCube(groundChecker.position,
            new Vector2(groundCheckBoxWidth, groundCheckDistance));
    }

#endif
}
