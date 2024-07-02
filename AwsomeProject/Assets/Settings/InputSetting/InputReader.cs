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

    #region QuickSlot

    public event Action FirstQuickSlot;
    public event Action SecondQuickSlot;
    public event Action ThirdQuickSlot;
    public event Action ForthQuickSlot;
    public event Action FifthQuickSlot;

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

    public void OnQuickSlotFirst(InputAction.CallbackContext context)
    {
        if (context.performed)
            FirstQuickSlot?.Invoke();
    }

    public void OnQuickSlotSecond(InputAction.CallbackContext context)
    {
        if (context.performed)
            SecondQuickSlot?.Invoke();
    }

    public void OnQuickSlotThird(InputAction.CallbackContext context)
    {
        if (context.performed)
            ThirdQuickSlot?.Invoke();
    }

    public void OnQuickSlotForth(InputAction.CallbackContext context)
    {
        if (context.performed)
            ForthQuickSlot?.Invoke();
    }

    public void OnQuickSlotFifth(InputAction.CallbackContext context)
    {
        if (context.performed)
            FifthQuickSlot?.Invoke();
    }

    public void OnUsePortion(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnTryUseQuickSlot?.Invoke();

        if (context.canceled)
            OnUseQuickSlot?.Invoke();
    }

    #endregion
}
