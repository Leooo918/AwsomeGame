using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingPortion : MonoBehaviour
{
    private Effect effect;
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;


    [SerializeField] private GameObject portionEffectPf;
    [SerializeField] private int maxEffectGatter = 10;
    [SerializeField] private float portionThrowingSpeed = 8f;
    [SerializeField] private float spinPower = 360f;
    private float currentRotation = 0;

    private Vector2 portionThrowingDirection;
    private Collider2D[] results;

    private void Awake()
    {
        spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        results = new Collider2D[maxEffectGatter];
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        currentRotation += spinPower * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //이것도 파티클 시스템 쓰면 그거 받아와서 암튼 풀링하고 뭐시기 해줘야도미
        Debug.Log(collision);
        Instantiate(portionEffectPf, transform.position, Quaternion.identity);

        //overlapcircle로 
        Physics2D.OverlapCircleNonAlloc(transform.position, 3, results);
        for (int i = 0; i < maxEffectGatter; i++)
        {
            if (results[i] == null) break;

            if (results[i].TryGetComponent<IGetPortionEffect>(out IGetPortionEffect entity))
                entity.GetEffort(effect);
        }

        Destroy(gameObject);
    }

    private IEnumerator DelayColliderOn()
    {
        if (collider != null)
            collider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        if (collider != null)
            collider.enabled = true;
    }

    public void Init(PortionItem portion)
    {
        this.effect = portion.portionEffect;
        spriteRenderer.sprite = portion.portionSprite;

        Vector3 mouseDir = 
            (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        portionThrowingDirection = mouseDir;
        rigidbody.AddForce(portionThrowingDirection * portionThrowingSpeed, ForceMode2D.Impulse);

        StartCoroutine(DelayColliderOn());
    }
}
