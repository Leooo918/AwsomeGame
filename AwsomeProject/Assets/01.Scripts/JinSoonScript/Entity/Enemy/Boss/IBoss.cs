

using Cinemachine;

public interface IBoss
{
    public bool IsBossDead { get; protected set; }

    public CinemachineVirtualCamera _bossRoomCam { get; set; }
    public CinemachineVirtualCamera _bossWatchingCam { get; set; }

    public void StartBoss();
    public void EndBoss();

    public void GoToNextPhase(int phase);
}