using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = transform.parent.GetComponent<PlayerAttack>();
        player = transform.parent.GetComponent<Player>();
    }

    public void AnimationFinishTrigger()
    {
        player.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        playerAttack.Attack();
    }
}
