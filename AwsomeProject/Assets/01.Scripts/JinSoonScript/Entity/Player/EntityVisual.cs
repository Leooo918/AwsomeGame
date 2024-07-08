using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class EntityVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material playerMat;
    [SerializeField] private float whiteTime = 0.15f;
    [SerializeField] private GameObject _skillEffect;

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

    public void SkillEffect()
    {
        _skillEffect.SetActive(true);
    }

    public void Hit()
    {
        StartCoroutine("HitRoutine");
    }

    private IEnumerator HitRoutine()
    {
        playerMat.SetFloat("_IsWhite", 1);
        yield return new WaitForSeconds(whiteTime);
        playerMat.SetFloat("_IsWhite", 0);
    }
}
