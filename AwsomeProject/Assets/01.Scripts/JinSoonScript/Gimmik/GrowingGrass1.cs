using UnityEngine;

public class GrowingGrass1 : MonoBehaviour, IGetPortionEffect
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField]private Transform _objTrm;
    [SerializeField] private float _growingSpeed = 20;
    private Rigidbody2D rb2d;
    private bool _isGrowing = false;

    private RaycastHit2D[] _coll;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _coll = new RaycastHit2D[1];
    }

    private void Update()
    {
        if (_isGrowing)
        {
            bool isRightDetectedWall = false;
            bool isLeftDetectedWall = false;
            float right = _objTrm.position.x + (transform.localScale.x / 2);
            float left = _objTrm.position.x - (transform.localScale.x / 2);

            transform.localScale += Vector3.one * _growingSpeed * Time.deltaTime;
            //transform.position += new Vector3(0, _growingSpeed * Time.deltaTime / 2, 0);

            Vector2 lPosition = new Vector2(left, _objTrm.position.y);
            Vector2 size = new Vector2(0.1f, transform.localScale.y - 0.2f);
            if (Physics2D.BoxCastNonAlloc(lPosition, size, 0, Vector2.left, _coll, 0.1f, _whatIsGround) > 0)
            {
                isLeftDetectedWall = true;
                transform.position += Vector3.right * _growingSpeed * Time.deltaTime;
            }

            Vector2 rPositoin = new Vector2(right, _objTrm.position.y);
            if (Physics2D.BoxCastNonAlloc(rPositoin, size, 0, Vector2.right, _coll, 0.1f, _whatIsGround) > 0)
            {
                isRightDetectedWall = true;
                transform.position += Vector3.left * _growingSpeed * Time.deltaTime;
            }

            if (isRightDetectedWall && isLeftDetectedWall)
                _isGrowing = false;

            if (transform.localScale.x > 3)
            {
                _isGrowing = false;
                transform.localScale = Vector3.one * 3;
            }

            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void OnDrawGizmosSelected()
    {
        float right = _objTrm.position.x + (transform.localScale.x / 2);
        Vector2 rSize = new Vector2(0.1f, transform.localScale.y - 0.2f);
        float left = _objTrm.position.x - (transform.localScale.x / 2);
        Vector2 lSize = new Vector2(0.1f, transform.localScale.y - 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(right, _objTrm.position.y), rSize);
        Gizmos.DrawWireCube(new Vector2(left, _objTrm.position.y), lSize);
    }

    public void GetEffort(Effect effect)
    {
        GrowthEffect growth = effect as GrowthEffect;

        if (growth == null) return;
        _isGrowing = true;
    }
}
