using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeSkillEffect : MonoBehaviour
{
    public void OnEndSkillEffect()
    {
        gameObject.SetActive(false);
    }
}
