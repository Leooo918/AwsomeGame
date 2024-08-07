using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeedBack : MonoBehaviour
{
    [SerializeField] private HitEffect hitEffect;

    [SerializeField] private float criticalFontSize = 12f;
    [SerializeField] private float normalHitFontSize = 7f;

    [SerializeField] private Color criticalColor;
    [SerializeField] private Color normalHitColor;

    private Health health;

    private void Awake()
    {
         health = GetComponent<Health>();
    }

    public void Hit()
    {
        HitEffect effect = Instantiate(hitEffect).GetComponent<HitEffect>();

        bool isCrit = health.hitData.isLastAttackCritical;
        float fontSize = isCrit ? criticalFontSize : normalHitFontSize;
        Color color = isCrit ? criticalColor : normalHitColor;
        Vector2 position = (Vector2)transform.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1.5f));

        effect.Init(health.hitData.lastAttackDamage, fontSize, color, position);
        effect.DoEffect();
    }
}
