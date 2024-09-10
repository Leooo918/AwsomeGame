using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVine : MonoBehaviour, IAffectable
{
    public void ApplyEffect()
    {
        Destroy(gameObject);
    }
}
