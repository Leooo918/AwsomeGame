using System.Collections.Generic;
using UnityEngine;

public class Projectary : MonoBehaviour
{
    private Player player;
    [SerializeField] private float _time;
    [SerializeField] private int _count;
    private float portionThrowingSpeed = 40f;

    [SerializeField]
    private GameObject _projectaryPrefab;

    private List<Transform> _projectileList = new List<Transform>();

    [SerializeField]
    private LayerMask _whatIsObstacle;
    private float _delta = 0;
    private bool _isDrawingProjectile = false;

    private void Awake()
    {
        SetData(_time, _count);
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.PlayerInput.OnTryUseQuickSlot += DrawProjectile;
        player.PlayerInput.OnUseQuickSlot += DisableProjectile;
    }

    private void OnDisable()
    {
        player.PlayerInput.OnTryUseQuickSlot -= DrawProjectile;
        player.PlayerInput.OnUseQuickSlot -= DisableProjectile;
    }

    private void Update()
    {
        if (_isDrawingProjectile)
        {
            Vector2 pos = transform.position;
            Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3)pos;
            mouseDir = mouseDir.normalized;
            Vector3 power = mouseDir * portionThrowingSpeed;
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
        _isDrawingProjectile = true;
    }

    public void DrawLine(Vector2 pos, Vector3 power)
    {
        //Debug.Log(power.x);
        bool flag = true;
        float gravity = Physics2D.gravity.y;
        for (int i = 1; i < _projectileList.Count; i++)
        {
            Transform t = _projectileList[i];
            if (flag)
            {
                SpriteRenderer renderer = t.GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                color.a = (float)(_projectileList.Count - i) / _projectileList.Count;
                renderer.color = color;

                t.gameObject.SetActive(true);

                Vector2 dotPos;
                float time = _delta * i;
                dotPos.x = pos.x + power.x * time;
                dotPos.y = pos.y + power.y * time + (gravity * Mathf.Pow(time, 2)) * 0.5f;

                if (Physics2D.OverlapCircle(dotPos, .3f, _whatIsObstacle))
                    flag = false;

                t.position = dotPos;
            }
            else
                t.gameObject.SetActive(false);
        }
    }

    public void DisableProjectile()
    {
        for (int i = 0; i < _projectileList.Count; i++)
        {
            _projectileList[i].gameObject.SetActive(false);
        }
        _isDrawingProjectile = false;
    }
}
