using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "SO/NormalScript")]
    public class NormalScriptSO : ScriptSO
    {
        public Character character;

        [HideInInspector]
        public ScriptSO nextScript;
    }

    [Serializable]
    public struct Character
    {
        public string name;
        public GameObject imgPrefab;
        [TextArea(3, 20)]
        public string talkDetails;
        //public CharacterAnimation animation;
    }
}
