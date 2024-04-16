using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSO : ScriptSO
{
    public List<string> options = new List<string>();
    public GameObject optionPf;

    public bool doTimer;
    public float selectTime = 5f;


    [HideInInspector]public ScriptSO[] nextScriptsByOption = new ScriptSO[3];
}
