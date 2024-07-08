using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : Singleton<InstantiateManager>
{
    [SerializeField] private GameObject ad;

    public void Instantiate(Transform trm)
    {
        Instantiate(ad, trm);
    }
}
