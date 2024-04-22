using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private Transform parent;
    private Vector3 origin;

    private void Awake()
    {
        parent = transform.parent;
        origin = transform.localScale;
    }

    private void Update()
    {
        if (parent.eulerAngles.y <= -180)
        {

            transform.localScale = new Vector3(-origin.x, origin.y, origin.z);
        }
        else
        {
            transform.localScale = origin;
        }
    }
}
