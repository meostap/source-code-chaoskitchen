using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteracAction;
    public event EventHandler OnInteraAlternatecAction;
    private PlayerInputActions playerInputAcitons;

    private void Awake()
    {
        // Remove the 'PlayerInputActions' declaration here to use the class-level variable
        playerInputAcitons = new PlayerInputActions();
        playerInputAcitons.Player.Enable();

        playerInputAcitons.Player.Interact.performed += Interact_performed;
        playerInputAcitons.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteraAlternatecAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteracAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAcitons.Player.Move.ReadValue<Vector2>();

       

        inputVector = inputVector.normalized;
        return inputVector;
    }
}