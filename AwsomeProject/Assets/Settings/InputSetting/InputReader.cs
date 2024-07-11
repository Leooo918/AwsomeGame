using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, Controlls.IPlayerActions
{
    #region input event

    public event Action JumpEvent;
    public event Action DashEvent;
    public event Action InteractPress;
    public event Action InteractRelease;
    public event Action PressTabEvent;
    public event Action AttackEvent;
    public event Action OpenOptionEvent;
    public event Action SelectMysteryPortion;

    #region QuickSlot

    public event Action<int> SelectQuickSlot;

    public event Action OnTryUseQuickSlot;
    public event Action OnUseQuickSlot;

    #endregion

    #endregion

    #region input value 

    public float XInput { get; private set; }
    public float YInput { get; private set; }

    #endregion

    private Controlls controlls;
    public Controlls Controlls => controlls;

    private void OnEnable()
    {
        if (controlls == null)
        {
            controlls = new Controlls();
            controlls.Player.SetCallbacks(this);
        }
        controlls.Player.Enable();
    }

    public void OnXMovement(InputAction.CallbackContext context)
    {
        XInput = context.ReadValue<float>();
    }

    public void OnYMovement(InputAction.CallbackContext context)
    {
        YInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            InteractPress?.Invoke();

        if (context.canceled)
            InteractRelease?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            DashEvent?.Invoke();
    }

    public void OnPressTab(InputAction.CallbackContext context)
    {
        if (context.performed)
            PressTabEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            AttackEvent?.Invoke();
    }
    public void OnOption(InputAction.CallbackContext context)
    {
        if (context.performed)
            OpenOptionEvent?.Invoke();
    }

    #region QuickSlots

    public void OnUsePortion(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnTryUseQuickSlot?.Invoke();

        if (context.canceled)
            OnUseQuickSlot?.Invoke();
    }

    public void OnSelectMysteryPortion(InputAction.CallbackContext context)
    {
        if(context.performed)
            SelectMysteryPortion?.Invoke();
    }

    public void OnSelectQuickSlot(InputAction.CallbackContext context)
    {
        int slotNum;

        if(context.performed && int.TryParse(context.control.name, out slotNum))
            SelectQuickSlot?.Invoke(slotNum - 1);
    }

    #endregion
}
