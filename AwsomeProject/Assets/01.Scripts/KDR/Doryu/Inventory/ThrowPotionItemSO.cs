using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Doryu/Item/ThrowPotionItem")]
public class ThrowPotionItemSO : PotionItemSO
{
    public int maxDetactEntity;
    public LayerMask whatIsEnemy;
    public int range;
}
