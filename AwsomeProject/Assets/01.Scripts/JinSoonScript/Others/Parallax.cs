using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    private GameObject cam;
    public bool isFastenY;  //y값을 고정해줄거냐?
    
    //대충 offset이라고 생각하면 되는데 곱연산으로 연산해줌
    //0이면 걍 안움직이고, 1이면 카메라속도에 딱 맞춰서 1보다 크면 더 빠르게, 더 작으면 더 느리게
    public float parallaxEffect;
    [SerializeField] private float height = 0;

    private void Awake()
    {
        startPos = transform.position.x;
        cam = Camera.main.gameObject;
        length = transform.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        //여기 위치로 이동시켜줌 대충 시작x 위치 + 저 카메라위치에 오프셋 적용시켜준거
        Vector3 targetPos = new Vector3(startPos + dist, height, transform.position.z);

        if (isFastenY)
            targetPos.y = cam.transform.position.y;

        transform.position = targetPos;

        //여기가 그 이미지 3개 번갈아면서 나오게 해주는 부분인www
        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
