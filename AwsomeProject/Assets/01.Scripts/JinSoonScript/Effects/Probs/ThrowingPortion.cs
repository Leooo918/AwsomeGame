using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ThrowingPortion : MonoBehaviour
{
    private Effect effect;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Player _player;


    private GameObject _portionEffectPf;
    [SerializeField] private int _maxEffectGatter = 10;
    [SerializeField] private float _portionThrowingSpeed = 40f;
    [SerializeField] private float _spinPower = 360f;
    private float _currentRotation = 0;

    private Vector2 _portionThrowingDirection;
    private Collider2D[] _results;

    private void Awake()
    {
        _player = PlayerManager.Instance.Player;
        _spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        _results = new Collider2D[_maxEffectGatter];
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        _currentRotation += _spinPower * Time.deltaTime;

        Vector3 direction = _portionThrowingDirection;
        direction.y = _rigidbody.velocity.y;
        _rigidbody.velocity = (direction);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //이것도 파티클 시스템 쓰면 그거 받아와서 암튼 풀링하고 뭐시기 해줘야도미
        Instantiate(_portionEffectPf, transform.position, Quaternion.identity);
        //CameraManager.Instance.ShakeCam(7f, 7f, 0.1f);

        //overlapcircle로 
        Physics2D.OverlapCircleNonAlloc(transform.position, 3, _results);
        for (int i = 0; i < _maxEffectGatter; i++)
        {
            if (_results[i] == null) break;

            if (_results[i].TryGetComponent(out IGetPortionEffect entity))
                entity.GetEffort(effect);
        }

        Destroy(gameObject);
    }

    private IEnumerator DelayColliderOn()
    {
        if (_collider != null)
            _collider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        if (_collider != null)
            _collider.enabled = true;
    }

    public void Init(PortionItem portion)
    {
        effect = portion.portionEffect;
        _spriteRenderer.sprite = portion.portionSprite;

        Vector3 mouseDir = 
            Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mouseDir.z = 0;
        mouseDir = mouseDir.normalized;

        PortionItemSO portionSO = portion.itemSO as PortionItemSO;
        _portionEffectPf = portionSO.portionParticle;
        
        _portionThrowingDirection = mouseDir * _portionThrowingSpeed;
        _portionThrowingDirection = _player.portionThrowingDir;
        //Debug.Log($"{_portionThrowingDirection}, {(_portionThrowingDirection * _portionThrowingSpeed).normalized}, {_portionThrowingDirection.magnitude}, {_portionThrowingDirection * _portionThrowingSpeed}, {_portionThrowingSpeed}, {(_portionThrowingDirection * _portionThrowingSpeed).magnitude}");
        _rigidbody.AddForce(_portionThrowingDirection, ForceMode2D.Impulse);

        StartCoroutine(DelayColliderOn());
    }
}
