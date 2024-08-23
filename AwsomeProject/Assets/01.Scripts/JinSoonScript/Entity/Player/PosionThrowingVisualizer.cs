using UnityEngine;

public class PosionThrowingVisualizer : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [Space(10)]

    [SerializeField] private GameObject _aimNothing;
    [SerializeField] private GameObject _aimEnemy;

    [Space(10)]
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private float _correction;

    [Space(10)]
    private float _throwingSpeed = 15f;
    [SerializeField] private float _gravityScale = -9.8f;

    private Player _player;
    private Transform _playerTrm;
    private Collider2D[] _coll;
    private RaycastHit2D[] _raycast;
    private Vector2 _throwingDir;

    private void Awake()
    {
        _player = PlayerManager.Instance.Player;
        _playerTrm = PlayerManager.Instance.PlayerTrm;
        _coll = new Collider2D[1];
        _raycast = new RaycastHit2D[1];
    }

    private void LateUpdate()
    {
        CheckMousePosition();
    }

    private void CheckMousePosition()
    {
        Vector2 mousePosition = _inputReader.MousePosition;

        int detectedEnemy = Physics2D.OverlapCircleNonAlloc(mousePosition, _correction, _coll, _whatIsEnemy);
        if (detectedEnemy > 0)
        {
            transform.position = _coll[0].transform.position + (Vector3)_coll[0].offset;
            transform.localScale = _coll[0].bounds.size * 1.2f;
            if (!_aimEnemy.activeSelf) _aimEnemy.SetActive(true);
            if (_aimNothing.activeSelf) _aimNothing.SetActive(false);
            _player.portionThrowingDir =
                CalculateThrowDirection(_playerTrm.position, _coll[0].transform.position);

            return;
        }


        int detectedGround = Physics2D.BoxCastNonAlloc
            (mousePosition, Vector2.one * _correction, 0, Vector2.zero, _raycast, _whatIsGround);

        //Vector2 mouseDir = (mousePosition - (Vector2)_playerTrm.position).normalized;
        //int detectedGround = Physics2D.RaycastNonAlloc(_playerTrm.position + Vector3.up, mouseDir, _raycast, Mathf.Infinity, _whatIsGround);
        //if (detectedGround > 0)
        //{
        //    Debug.Log("땅");
        //    transform.position = _raycast[0].point;
        //    transform.localScale = Vector3.one;
        //    if (_aimEnemy.activeSelf) _aimEnemy.SetActive(false);
        //    if (!_aimNothing.activeSelf) _aimNothing.SetActive(true);

        //    return;
        //}

        if (_aimEnemy.activeSelf) _aimEnemy.SetActive(false);
        if (!_aimNothing.activeSelf) _aimNothing.SetActive(true);
        transform.position = mousePosition;
        transform.localScale = Vector3.one;

        //_throwingSpeed = CalculateLaunchSpeed(_playerTrm.position, mousePosition);

        _player.portionThrowingDir =
            CalculateThrowDirection(_playerTrm.position + _player.ThrowingOffset, mousePosition);
    }


    //float CalculateLaunchSpeed(Vector2 start, Vector2 end)
    //{
    //    Vector2 distance = end - start;
    //    float dx = distance.x;
    //    float dy = distance.y;

    //    // 최적의 발사 속도 계산을 위해 각도 45도 가정
    //    float theta = 45 * Mathf.Deg2Rad;// Mathf.Atan2(dy, dx);

    //    // x와 y 방향에서 속도 성분 계산
    //    float tanTheta = Mathf.Tan(theta);
    //    float num = Mathf.Sqrt(_gravityScale * dx * dx / (2 * (dx * tanTheta - dy)));
    //    float denom = Mathf.Sqrt(1 + tanTheta * tanTheta);

    //    return num / denom;
    //}

    private Vector2 CalculateThrowDirection(Vector2 startPosition, Vector2 targetPosition)
    {
        Vector2 targetDir = targetPosition - startPosition;
        _throwingSpeed = targetDir.magnitude * Mathf.Abs(_gravityScale) / 9.8f;

        float dx = targetDir.x;
        float dy = targetDir.y;

        float time = Mathf.Abs(dx) / _throwingSpeed;

        float vx = dx / time;
        float vy = (dy + .5f * -_gravityScale * Mathf.Pow(time, 2)) / time;

        _throwingDir = new Vector2(vx, vy).normalized;
        return _throwingDir * _throwingSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(_playerTrm.position + _player.ThrowingOffset, _throwingDir);
    }
}
