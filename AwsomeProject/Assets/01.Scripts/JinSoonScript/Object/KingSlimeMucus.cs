using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeMucus : MonoBehaviour
{
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    public void Fire(Vector2 direction)
    {
        rigid.AddForce(direction, ForceMode2D.Impulse);
    }
}
