using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            MapManager.Instance.DisableMap(player.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            MapManager.Instance.EnableMap(player.transform.position);
        }
    }
}
