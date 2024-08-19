using UnityEngine;

public class GrowingGrass2 : MonoBehaviour, IGetPortionEffect
{


    public void GetEffort(Effect effect)
    {
        GrowthEffect growth = effect as GrowthEffect;

        if (growth == null) return;
    }
}
