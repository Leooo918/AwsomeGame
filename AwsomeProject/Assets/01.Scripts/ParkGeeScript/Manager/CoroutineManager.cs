using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager>
{
    public void StartManagedCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
