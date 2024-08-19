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
                playerTrm = Player.transform;

            return playerTrm;
        }
        private set
        {
            playerTrm = value;
        }
    }

    public void DisableAllPlayerInput()
    {
        player.PlayerInput.Controlls.asset.FindAction("XMovement").Disable();
        player.PlayerInput.Controlls.asset.FindAction("YMovement").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Jump").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Interact").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Dash").Disable();
        player.PlayerInput.Controlls.asset.FindAction("PressTab").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Attack").Disable();
        player.PlayerInput.Controlls.asset.FindAction("UsePortion").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Option").Disable();
        player.PlayerInput.Controlls.asset.FindAction("SelectMysteryPortion").Disable();
        player.PlayerInput.Controlls.asset.FindAction("SelectQuickSlot").Disable();
    }

    public void EnableAllPlayerInput()
    {
        player.PlayerInput.Controlls.asset.FindAction("XMovement").Enable();
        player.PlayerInput.Controlls.asset.FindAction("YMovement").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Jump").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Interact").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Dash").Enable();
        player.PlayerInput.Controlls.asset.FindAction("PressTab").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Attack").Enable();
        player.PlayerInput.Controlls.asset.FindAction("UsePortion").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Option").Enable();
        player.PlayerInput.Controlls.asset.FindAction("SelectMysteryPortion").Enable();
        player.PlayerInput.Controlls.asset.FindAction("SelectQuickSlot").Enable();
    }

    public void DisablePlayerMovementInput()
    {

        player.PlayerInput.Controlls.asset.FindAction("XMovement").Disable();
        player.PlayerInput.Controlls.asset.FindAction("YMovement").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Jump").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Dash").Disable();
        player.PlayerInput.Controlls.asset.FindAction("Attack").Disable();
        player.PlayerInput.Controlls.asset.FindAction("UsePortion").Disable();
    }

    public void EnablePlayerMovementInput()
    {
        player.PlayerInput.Controlls.asset.FindAction("XMovement").Enable();
        player.PlayerInput.Controlls.asset.FindAction("YMovement").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Jump").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Dash").Enable();
        player.PlayerInput.Controlls.asset.FindAction("Attack").Enable();
        player.PlayerInput.Controlls.asset.FindAction("UsePortion").Enable();
    }

    public void EnablePlayerInventoryInput()
    {
        player.PlayerInput.Controlls.asset.FindAction("PressTab").Enable();
        player.PlayerInput.Controlls.asset.FindAction("SelectMysteryPortion").Enable();
        player.PlayerInput.Controlls.asset.FindAction("SelectQuickSlot").Enable();
    }

    public void DisablePlayerInventoryInput()
    {
        player.PlayerInput.Controlls.asset.FindAction("PressTab").Disable();
        player.PlayerInput.Controlls.asset.FindAction("SelectMysteryPortion").Disable();
        player.PlayerInput.Controlls.asset.FindAction("SelectQuickSlot").Disable();
    }
}
