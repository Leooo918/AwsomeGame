using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    [HideInInspector]public IngameQuickSlot curSelectingPortion;
    public QuickSlotVisualizer visualizer;
    
    public void EnableQuickSlot() => visualizer.EnableQuickSlot();
    public void DisableQuickSlot() => visualizer.DisableQuickSlot();
}
