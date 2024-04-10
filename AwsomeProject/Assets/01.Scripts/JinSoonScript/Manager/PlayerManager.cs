using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Player player { get; private set; }
    public Transform playerTrm {  get; private set; }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerTrm = player.transform;
    }
}
