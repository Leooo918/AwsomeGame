using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeAnimationTrigger : MonoBehaviour
{
    private KingSlime kingSlime;

    private void Awake()
    {
        kingSlime = GetComponentInParent<KingSlime>();
    }

    public void AnimationFinishTrigger()
    {
        kingSlime.AnimationFinishTrigger();
    }
}
