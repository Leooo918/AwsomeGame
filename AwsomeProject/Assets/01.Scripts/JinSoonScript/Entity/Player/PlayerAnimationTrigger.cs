using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;
    private EntityAttack entityAttack;

    private void Awake()
    {
        entityAttack = transform.parent.GetComponent<EntityAttack>();
        player = transform.parent.GetComponent<Player>();
    }

    public void AnimationFinishTrigger()
    {
        player.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        entityAttack.Attack();
    }
}
