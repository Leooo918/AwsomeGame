using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VineState
{
    Growing,
    Grown,
    Shrinking,
    Shrunk,
}

public class GrowingGrass : MonoBehaviour
{
    public VineState CurrentState { get; private set; } = VineState.Grown;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
