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
    public event Action Interact;
    public event Action DashEvent;

    #endregion

    #region input value 

    public float XInput {  get; private set; }
    public float YInput {  get; private set; }

    #endregion

    private Controlls controlls;

    private void OnEnable()
    {
        if(controlls == null) 
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
            Interact?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
            DashEvent?.Invoke();
    }
}
