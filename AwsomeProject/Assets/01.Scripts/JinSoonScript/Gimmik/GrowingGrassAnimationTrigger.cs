using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingGrassAnimationTrigger : MonoBehaviour
{
    private Animator _animator;
    private GrowingGrass growingGrass;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        growingGrass = GetComponentInParent<GrowingGrass>();
    }

    public void GrowStart()
    {
        Debug.Log("นึ");
        growingGrass.GrowStart();
    }
}
