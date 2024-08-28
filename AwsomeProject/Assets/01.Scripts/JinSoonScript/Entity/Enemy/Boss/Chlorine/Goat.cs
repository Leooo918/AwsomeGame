using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoatStateEnum 
{
    Idle,

    Dead
}


public class Goat : Enemy<GoatStateEnum>, IBoss
{
    public CinemachineVirtualCamera _bossRoomCam { get ; set ; }
    public CinemachineVirtualCamera _bossWatchingCam { get ; set ; }
    bool IBoss.IsBossDead { get ; set ; }

    public void EndBoss()
    {

    }

    public void GoToNextPhase(int phase)
    {

    }

    public void StartBoss()
    {

    }
}
