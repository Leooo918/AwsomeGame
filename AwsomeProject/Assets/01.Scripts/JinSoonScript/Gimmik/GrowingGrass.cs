using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GrowingGrass : MonoBehaviour, IGetPortionEffect
{
    //private 
    private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private SpriteRenderer _visualTemp;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private float _rayDistance = 8f;
    [SerializeField] private float _resetDelay = 3f;

    private GrowingDirection _direction;
    private bool _isGrowing = false;
    private bool _isEndGrow = false;
    private bool _canClimb = false;

    private float _currentScale;
    private float _growingSpeed = 9f;
    private float _maxGrowingSize = 7.5f;

    private Vector2 _originColliderSize;
    private Vector2 _originColliderOffset;

    private int _growingAnimationHash = Animator.StringToHash("GrowStart");
    private int _growingResetHash = Animator.StringToHash("GrowReset");
    private Sequence _resetSeq;

    private void Awake()
    {
        _direction = (GrowingDirection)(int)(transform.eulerAngles.z / 90f);
        _collider = GetComponent<BoxCollider2D>();
        _currentScale = _collider.size.y;
        _canClimb = _direction == GrowingDirection.Up;

        _originColliderOffset = _collider.offset;
        _originColliderSize = _collider.size;
    }

    private void Update()
    {
        Vector2 rayDir = transform.up;
        float distance = _maxGrowingSize;
        //rayDir = Quaternion.Euler(0, 0, 90 * (int)_direction) * rayDir;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 15, _whatIsGround);
        //if (hit.collider != null)
        //{
        //    distance = hit.distance;
        //    _maxGrowingSize = distance;
        //}
        Debug.DrawRay(transform.position, rayDir * distance);


        if (_isGrowing == false) return;

        _collider.size = new Vector2(0.76f, _currentScale);
        _collider.offset = new Vector2(0f, _currentScale / 2);
        _currentScale += Time.deltaTime * _growingSpeed;

        if (_currentScale > _maxGrowingSize)
        {
            _animator.speed = 0;
            _isGrowing = false;
            _isEndGrow = true;
        }
    }

    public void GetEffort(Effect effect)
    {
        GrowthEffect growth = effect as GrowthEffect;
        if (growth == null || _isEndGrow) return;

        _animator.SetTrigger(_growingAnimationHash);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canClimb && collision.TryGetComponent(out Player player))
        {
            player.Climb(true);
        }

        if (collision.TryGetComponent(out IDamageable enemy) && _isGrowing)
        {
            //enemy.Rape(3f);
            enemy.TakeDamage(0, Vector2.zero, null);
            //여기에 적 속박 시키게 하는 코드
        }

        //땅에 닿았다면 자라나는 걸 멈추게 해야함
        if (collision.CompareTag("Ground"))
        {
            _isEndGrow = true;
            _isGrowing = false;
            _collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_canClimb && collision.TryGetComponent(out Player player))
        {
            player.Climb(false);
        }
    }

    public void GrowStart()
    {
        _isGrowing = true;

        Vector2 rayDir = Vector2.up;
        rayDir = Quaternion.Euler(0, 0, 90 * (int)_direction) * rayDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, _rayDistance, _whatIsGround);
        Debug.DrawRay(transform.position, rayDir * 15);

        if (hit.collider != null) _maxGrowingSize = hit.distance + 0.8f;

        StartCoroutine(DelayResetGrowing());
    }

    public void ResetGrowing()
    {
        _animator.speed = 1;
        _collider.size = _originColliderSize;
        _collider.offset = _originColliderOffset;
        _currentScale = _originColliderSize.y;

        _isGrowing = false;
        _isEndGrow = false;
        _canClimb = false;

        if (_resetSeq != null && _resetSeq.active)
            _resetSeq.Kill();

        _resetSeq = DOTween.Sequence();

        _resetSeq.Append(_visual.DOFade(0, 0.5f).SetEase(Ease.Linear))
            .InsertCallback(0.3f, () => _visualTemp.enabled = true)
            .AppendCallback(() => _animator.SetTrigger(_growingResetHash))
            .AppendInterval(0.15f)
            .AppendCallback(() =>
            {
                _visualTemp.enabled = false;
                _visual.color = Color.white;
            });
    }

    private IEnumerator DelayResetGrowing()
    {
        yield return new WaitForSeconds(_resetDelay);
        ResetGrowing();
    }
}

public enum GrowingDirection
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}