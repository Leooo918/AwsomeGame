using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "SO/Script")]
    public abstract class ScriptSO : ScriptableObject
    {
        [Header("Camera")]
        public CameraSetting[] cameraSettings;

        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
    }

    [Serializable]
    public class CameraSetting
    {
        public float delayBeforeStart;

        [Space(16)]
        public bool swapCameras;
        public bool panCamera;

        [Header("OnSwapCemeras")]
        [Tooltip("NameOfCamera")]
        public string changeCameraName;

        [Header("PanCamera")]
        public bool returnToStartPos;
        public PanDirection panDirection;
        public float panDistance;
        public float panTime;
    }
}
