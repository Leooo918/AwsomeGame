using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyDialogSystem
{
    public class LikelyCondition : Condition
    {
        private DialogStatus status = null;

        [SerializeField] private float leastLikelyValue;

        public override bool Judge()
        {
            status = DialogManager.instance.Status;

            return false;
        }
    }
}
