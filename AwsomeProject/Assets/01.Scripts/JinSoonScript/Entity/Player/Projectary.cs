using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Projectary : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _time;
    [SerializeField] private float _correction; //보정
    [SerializeField] private float _gravityScale = 9.8f;
    [SerializeField] private float _throwingSpeed = 40f;
    [SerializeField] private int _count;
    [SerializeField] private Vector2 _offset = new Vector2(0, 0.5f);

    [SerializeField] private GameObject _projectaryPrefab;

    private List<Transform> _projectileList = new List<Transform>();

    [SerializeField]
    private LayerMask _whatIsObstacle, _whatIsEnemy;
    private float _delta = 0;
    private bool _isDrawingProjectile = false;
    private Collider2D[] _colls;

    private void Awake()
    {
        _colls = new Collider2D[1];
        SetData(_time, _count);
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _player.PlayerInput.OnTryUseQuickSlot += DrawProjectile;
        _player.PlayerInput.OnUseQuickSlot += DisableProjectile;
    }

    private void OnDisable()
    {
        _player.PlayerInput.OnTryUseQuickSlot -= DrawProjectile;
        _player.PlayerInput.OnUseQuickSlot -= DisableProjectile;
    }

    private void Update()
    {
        if (_isDrawingProjectile)
        {
            Vector2 pos = (Vector2)transform.position + _offset;
            Vector2 mouseDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3)pos);
            mouseDir = mouseDir.normalized;

            Vector3 power = mouseDir * _throwingSpeed;
            DrawLine(pos, power);
        }
    }

    public void SetData(float time, int count)
    {
        _time = time;
        _count = count;
        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(_projectaryPrefab, transform);
            _projectileList.Add(g.transform);
            g.SetActive(false);
        }
        _delta = _time / _count;
    }

    public void DrawProjectile()
    {
        if (_player.throwingPortionSelected == false) return;

        _isDrawingProjectile = true;
    }

    public void DisableProjectile()
    {
        for (int i = 0; i < _projectileList.Count; i++)
        {
            _projectileList[i].gameObject.SetActive(false);
        }
        _isDrawingProjectile = false;
    }

    public void DrawLine(Vector2 pos, Vector3 power)
    {
        bool flag = true;
        float gravity = _gravityScale;

        for (int i = 1; i < _projectileList.Count; i++)
        {
            Transform t = _projectileList[i];
            if (flag == false)
            {
                t.gameObject.SetActive(false);
                break;
            }

            Vector2 dotPos;
            float time = _delta * i;
            dotPos.x = pos.x + power.x * time;
            dotPos.y = pos.y + power.y * time + (gravity * Mathf.Pow(time, 2));

            //만약 주변에 적이 있다면
            if (Physics2D.OverlapCircleNonAlloc(dotPos, _correction, _colls, _whatIsEnemy) > 0)
            {
                Vector2 tmpPower = power;
                power = CalculateThrowDirection(pos, _colls[0].transform.position, _throwingSpeed);
                flag = false;
            }
        }
        flag = true;

        for (int i = 1; i < _projectileList.Count; i++)
        {
            Transform t = _projectileList[i];
            if (flag == false)
            {
                t.gameObject.SetActive(false);
                break;
            }

            SpriteRenderer renderer = t.GetComponent<SpriteRenderer>();
            Color color = renderer.color;
            color.a = (float)(_projectileList.Count - i) / _projectileList.Count;
            renderer.color = color;

            t.gameObject.SetActive(true);

            Vector2 dotPos;
            float time = _delta * i;
            dotPos.x = pos.x + power.x * time;
            dotPos.y = pos.y + power.y * time + (gravity * Mathf.Pow(time, 2));

            if (Physics2D.OverlapCircleNonAlloc(dotPos, 0.3f, _colls, _whatIsObstacle) > 0)
                flag = false;

            t.position = dotPos;
        }

        _player.PortionThrowingDir = (power * 2); 
    }

    private Vector2 CalculateThrowDirection(Vector2 startPosition, Vector2 targetPosition, float initialSpeed)
    {
        //// 수평 거리와 수직 거리 계산
        //Vector2 horizontalDistance = new Vector2(target.x - start.x, 0);
        //float verticalDistance = target.y - start.y;

        //// 수평 거리의 크기 계산
        //float horizontalDistanceMagnitude = horizontalDistance.magnitude;

        //// 포션이 날아가는 시간을 계산 (단순히 중력과 초기 속도를 고려)
        //float time = horizontalDistanceMagnitude / initialSpeed;

        //// 초기 속도를 계산
        //float initialVerticalSpeed = (verticalDistance + 0.5f * _gravityScale * Mathf.Pow(time, 2)) / time;

        //// 초기 속도 벡터 구성
        //Vector2 initialVelocity = horizontalDistance.normalized * initialSpeed;
        //initialVelocity.y = initialVerticalSpeed;

        //Debug.Log(start + " " + target + ": " + initialSpeed + " " + initialVelocity);
        //return initialVelocity;

        //Vector2 displacement = targetPosition - startPosition;
        //float distanceX = displacement.x;
        //float distanceY = displacement.y;

        //// 수평 거리
        //float distance = displacement.magnitude;

        //// 초기 각도를 계산하기 위해 quadratic formula를 사용하여 y = v*sin(theta) - (g*t^2)/2
        //float angle = Mathf.Atan2(distanceY + (0.5f * _gravityScale * (distance / initialSpeed) * (distance / initialSpeed)), distanceX);

        //// 초기 속도 벡터를 계산
        //float initialVelocityX = initialSpeed * Mathf.Cos(angle);
        //float initialVelocityY = initialSpeed * Mathf.Sin(angle);

        //// 속도 성분을 하나의 벡터로 결합
        //Vector2 initialVelocity = new Vector2(initialVelocityX, initialVelocityY);

        Vector2 offset = new Vector2(0, 2);
        Vector2 initialVelocity = ((targetPosition - startPosition) + offset).normalized * initialSpeed;
        return initialVelocity;    
    }
}
