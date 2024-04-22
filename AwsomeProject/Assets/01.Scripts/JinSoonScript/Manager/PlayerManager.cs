using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private Player player;
    private Transform playerTrm;

    public Player Player 
    {
        get
        {
            if(player == null)
                player = FindObjectOfType<Player>();

            if (player == null)
                Debug.LogError($"Player is not exist in this scene but still trying to excess it");

            return player;
        } 
        private set
        {
            player = value;
        }
    }
    public Transform PlayerTrm
    {
        get
        {
            if (playerTrm == null)
                playerTrm = player.transform;

            return playerTrm;
        }
        private set
        {
            playerTrm = value;
        }
    }
}
