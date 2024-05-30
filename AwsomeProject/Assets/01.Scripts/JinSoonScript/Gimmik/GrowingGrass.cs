using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingGrass : MonoBehaviour, IGetPortionEffect
{
    //private 
    private Collider2D collider;
    [SerializeField] private LayerMask whatIsGround;

    private GrowingDirection direction;
    private bool isGrowing = false;
    private bool isEndGrow = false;

    private float currentScale = 1;
    private float growingSpeed = 7;
    private float maxGrowingSize = 15;


    private void Awake()
    {
        direction = (GrowingDirection)(int)(transform.eulerAngles.z / 90f);
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 rayDir = Vector2.up;
        rayDir = Quaternion.Euler(0, 0, 90 * (int)direction) * rayDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 15, whatIsGround);
        Debug.DrawRay(transform.position, rayDir * hit.distance);

        if (isGrowing == false) return;

        transform.localScale = new Vector3(1, currentScale, 1);
        currentScale += growingSpeed * Time.deltaTime;

        if (currentScale > maxGrowingSize)
        {
            isGrowing = false;
            isEndGrow = true;
        }
    }

    public void GetEffort(Effect effect)
    {
        GrowthEffect growth = effect as GrowthEffect;
        if (growth == null || isEndGrow) return;

        isGrowing = true;

        Vector2 rayDir = Vector2.up;
        rayDir = Quaternion.Euler(0, 0, 90 * (int)direction) * rayDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 15, whatIsGround);
        Debug.DrawRay(transform.position, rayDir * 15);

        if(hit.collider != null)
        {
            maxGrowingSize = hit.distance + 0.8f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.TryGetComponent(out IDamageable enemy))
        {
            //여기에 적 속박 시키게 하는 코드
        }

        //땅에 닿았다면 자라나는 걸 멈추게 해야함
        if (collision.CompareTag("Ground"))
        {
            isEndGrow = true;
            isGrowing = false;
            collider.enabled = false;
        }
    }
}

public enum GrowingDirection
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}