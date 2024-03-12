using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Probs
{
    [SerializeField] private float jumpHeight;
    private Animator animator;
    private int animHash;

    private float delay = 0.5f;
    private bool isActive = false;

    private void Awake()
    {
        animator = transform.Find("Visual").GetComponent<Animator>();
        animHash = Animator.StringToHash("Active");
    }

    public override void Interact(Entity entity)
    {
        if (isActive) return;

        StartCoroutine("DelayActive");
        animator.SetTrigger(animHash);
        entity.SetVelocity(entity.rigidbodyCompo.velocity.x, jumpHeight);
    }

    private IEnumerator DelayActive()
    {
        isActive = true;
        yield return new WaitForSeconds(delay);
        isActive = false;
    }
}