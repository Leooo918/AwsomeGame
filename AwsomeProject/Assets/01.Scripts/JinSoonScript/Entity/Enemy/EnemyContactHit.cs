using UnityEngine;

public class EnemyContactHit : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _knockBackPower;
    [SerializeField] private Vector3 _cameraShakeData;
    private Entity _owner;


    private void Awake()
    {
        _owner = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            Vector2 knockPower = _knockBackPower;
            knockPower.x *= _owner.FacingDir;

            player.healthCompo.TakeDamage(_damage, knockPower, _owner);
            CameraManager.Instance.ShakeCam(_cameraShakeData.x, _cameraShakeData.y, _cameraShakeData.z);
        }
    }
}
