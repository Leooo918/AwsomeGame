using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    private GameObject cam;
    public bool isFastenY;
    public float parallaxEffect;
    [SerializeField] private float height = 0;

    private void Awake()
    {
        startPos = transform.position.x;
        cam = Camera.main.gameObject;
        length = transform.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        Vector3 targetPos = new Vector3(startPos + dist, height, transform.position.z);

        if(isFastenY)
        {
            targetPos.y = cam.transform.position.y;
        }

        transform.position = targetPos;

        if (temp > startPos + length) startPos += length;
        else if(temp < startPos - length) startPos -= length;
    }
}
