using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMap : MonoBehaviour
{
    private readonly Vector3 offset = Vector2.up;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MapManager.Instance.DisableMap(collision.transform.position + offset);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MapManager.Instance.EnableMap(collision.transform.position + offset);
    }
}
