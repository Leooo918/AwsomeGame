using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarning : MonoBehaviour
{
    [SerializeField] private float _maxDetectingDistance;
    [SerializeField] private float _offset = 0.1f;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private Transform _warningImage;

    private RaycastHit2D[] _hit = new RaycastHit2D[1];


    private void Update()
    {
        if(Physics2D.BoxCastNonAlloc(transform.position, transform.localScale, 0, Vector2.down, _hit, _maxDetectingDistance, _whatIsGround) > 0)
        {
            float dist = _hit[0].distance;
            Vector2 position = transform.position;
            position.y -= dist + _offset;
            _warningImage.position = position;
        }
    }
}
