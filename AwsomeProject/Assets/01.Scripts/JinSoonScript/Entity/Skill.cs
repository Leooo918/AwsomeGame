using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public Entity owner;

    public abstract void UseSkill();

    public virtual void SetOwner(Entity owner)
    {
        this.owner = owner;
    }
}
