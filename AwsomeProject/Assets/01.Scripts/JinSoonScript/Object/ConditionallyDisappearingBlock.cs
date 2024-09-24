using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionallyDisappearingBlock : MonoBehaviour
{
    public Entity entity;

    private void Update()
    {
        if(entity.IsDead)
        {
            Destroy(gameObject);
        }
    }
}
