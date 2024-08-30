using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Projectary : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _time;
    [SerializeField] private float _correction; //����
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

            //���� �ֺ��� ���� �ִٸ�
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
        //// ���� �Ÿ��� ���� �Ÿ� ���
        //Vector2 horizontalDistance = new Vector2(target.x - start.x, 0);
        //float verticalDistance = target.y - start.y;

        //// ���� �Ÿ��� ũ�� ���
        //float horizontalDistanceMagnitude = horizontalDistance.magnitude;

        //// ������ ���ư��� �ð��� ��� (�ܼ��� �߷°� �ʱ� �ӵ��� ���)
        //float time = horizontalDistanceMagnitude / initialSpeed;

        //// �ʱ� �ӵ��� ���
        //float initialVerticalSpeed = (verticalDistance + 0.5f * _gravityScale * Mathf.Pow(time, 2)) / time;

        //// �ʱ� �ӵ� ���� ����
        //Vector2 initialVelocity = horizontalDistance.normalized * initialSpeed;
        //initialVelocity.y = initialVerticalSpeed;

        //Debug.Log(start + " " + target + ": " + initialSpeed + " " + initialVelocity);
        //return initialVelocity;

        //Vector2 displacement = targetPosition - startPosition;
        //float distanceX = displacement.x;
        //float distanceY = displacement.y;

        //// ���� �Ÿ�
        //float distance = displacement.magnitude;

        //// �ʱ� ������ ����ϱ� ���� quadratic formula�� ����Ͽ� y = v*sin(theta) - (g*t^2)/2
        //float angle = Mathf.Atan2(distanceY + (0.5f * _gravityScale * (distance / initialSpeed) * (distance / initialSpeed)), distanceX);

        //// �ʱ� �ӵ� ���͸� ���
        //float initialVelocityX = initialSpeed * Mathf.Cos(angle);
        //float initialVelocityY = initialSpeed * Mathf.Sin(angle);

        //// �ӵ� ������ �ϳ��� ���ͷ� ����
        //Vector2 initialVelocity = new Vector2(initialVelocityX, initialVelocityY);

        Vector2 offset = new Vector2(0, 2);
        Vector2 initialVelocity = ((targetPosition - startPosition) + offset).normalized * initialSpeed;
        return initialVelocity;    
    }
}
