using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMap : MonoBehaviour
{
    private readonly Vector3 offset = Vector2.up * 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("밍밍밍");
        MapManager.Instance.DisableMap(collision.transform.position + offset);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("밍");

        MapManager.Instance.EnableMap(collision.transform.position + offset);
    }
}
