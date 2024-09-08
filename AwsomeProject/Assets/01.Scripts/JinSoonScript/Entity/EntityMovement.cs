using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    protected Entity _owner;
    public Rigidbody2D RigidbodyCompo { get; protected set; }
    protected Vector2 _velocity;

    public void Initialize(Entity owner)
    {
        _owner = owner;
        RigidbodyCompo = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        RigidbodyCompo.velocity = _velocity;
    }

    public virtual void SetVelocity(Vector2 velocity, bool doNotFlip = false)
    {
        _velocity = velocity;
        if (!doNotFlip)
        {
            _owner.FlipController(velocity.x);
        }
    }

    public virtual void StopImmediately(bool withYAxis = false)
    {
        _velocity.x = 0;
        if (withYAxis)
        {
            _velocity = Vector2.zero;
        }
    }
}
