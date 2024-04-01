using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
    }

    public void AnimationFinishTrigger()
    {
        player.AnimationFinishTrigger();
    }
}
