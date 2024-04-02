using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeriVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material playerMat;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMat = spriteRenderer.material;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Hit();
        }
    }

    public void Hit()
    {
        StartCoroutine("HitRoutine");
    }

    private IEnumerator HitRoutine()
    {
        playerMat.SetFloat("_IsWhite", 1);
        yield return new WaitForSeconds(0.05f);
        playerMat.SetFloat("_IsWhite", 0);
    }
}
