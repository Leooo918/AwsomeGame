using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private List<CinemachineVirtualCamera> cameraSet;
    [SerializeField] private PlayerFollowObj follow;            //플레이어 따라가는 뇨속

    private Vector2 startingTrackedObjectOffset;
    private Tween panCameraTween;

    private CinemachineVirtualCamera currentCam;                //현재 카메라
    private CinemachineFramingTransposer framingTransposer;     //카메라 움직여주는 놈
    private CinemachineConfiner2D currentConfiner;              //카메라 범위 제한
    private CinemachineBasicMultiChannelPerlin currentPerline;  //카메라 흔들어 주는 놈

    private float shakeTime = 0;

    private Vector2[] dirArr = new Vector2[4] 
    { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;

        if (cameraSet.Count > 0)
            ChangeCam(cameraSet[0]);
    }

    public void ChangeCam(CinemachineVirtualCamera activeCam)
    {
        cameraSet.ForEach(x => x.Priority = 5);

        activeCam.Priority = 10;
        currentCam = activeCam;

        currentConfiner = 
            currentCam.GetComponent<CinemachineConfiner2D>();
        framingTransposer = currentCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        currentPerline = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        currentCam.Follow = follow.transform;
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection direction, bool panToStartingPos)
    {
        Vector3 endPos = Vector3.zero;

        if (!panToStartingPos)
        {
            endPos = 
                dirArr[(int)direction] * panDistance + startingTrackedObjectOffset;
        }
        else
            endPos = startingTrackedObjectOffset;
        

        if (panCameraTween != null && panCameraTween.IsActive())
            panCameraTween.Kill();


        panCameraTween = DOTween.To(
            () => framingTransposer.m_TrackedObjectOffset,
            value => framingTransposer.m_TrackedObjectOffset = value,
            endPos, panTime);
    }

    public void ChangeConfinder(PolygonCollider2D collider)
    {
        currentConfiner.m_BoundingShape2D = collider;
    }

    public void ShakeCam(float amplitude, float frequency, float time)
    {
        currentPerline.m_AmplitudeGain = amplitude;
        currentPerline.m_FrequencyGain = frequency;
        shakeTime = time;

        StartCoroutine(DelayStopShake());
    }

    private IEnumerator DelayStopShake()
    {
        yield return new WaitForSeconds(shakeTime);

        currentPerline.m_AmplitudeGain = 0f;
        currentPerline.m_FrequencyGain = 0f;
    }

}
