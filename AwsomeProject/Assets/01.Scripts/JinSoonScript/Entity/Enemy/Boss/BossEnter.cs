using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnter : MonoBehaviour
{
    public Entity boss;
    public GameObject frontWall;
    public GameObject backWall;

    public void StartBoss()
    {
        backWall.SetActive(true);
        boss.gameObject.SetActive(true);
    }

    public void EndBoss()
    {
        frontWall.SetActive(false);
    }
}
