using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public static QuickSlotManager Instance;

    public IngameQuickSlot curSelectingPortion;

    private void Awake()
    {
        if(Instance != null)
            Destroy(Instance);

        Instance = this;
    }


}
