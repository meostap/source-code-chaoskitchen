using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteracAction;
    public event EventHandler OnInteraAlternatecAction;
    public event EventHandler OnPauseAcitons;
    private PlayerInputActions playerInputAcitons;
   

    public enum Binding
    {
        Move_UP,
        Move_DOWN,
        Move_LEFT,
        Move_RIGHT,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause,


    }
    private void Awake()
    {
        Instance = this;
        // Remove the 'PlayerInputActions' declaration here to use the class-level variable
        playerInputAcitons = new PlayerInputActions();
        playerInputAcitons.Player.Enable();

        playerInputAcitons.Player.Interact.performed += Interact_performed;
        playerInputAcitons.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAcitons.Player.Pause.performed += Pause_performed;

        Debug.Log(GetBindingText(Binding.Move_UP));
    }

    private void OnDestroy()
    {
        playerInputAcitons.Player.Interact.performed -= Interact_performed;
        playerInputAcitons.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAcitons.Player.Pause.performed -= Pause_performed;

        playerInputAcitons.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAcitons?.Invoke(this,EventArgs.Empty);
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

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default :
            case Binding.Move_UP:
                return playerInputAcitons.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_DOWN:
                return playerInputAcitons.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_LEFT:
                return playerInputAcitons.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_RIGHT:
                return playerInputAcitons.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
              return  playerInputAcitons.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAcitons.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAcitons.Player.Pause.bindings[0].ToDisplayString();

        }
    }
    public void RebindBinding(Binding binding)
    {
        playerInputAcitons.Player.Disable();

        playerInputAcitons.Player.Move.PerformInteractiveRebinding(1)
            .OnComplete((callback) =>
            {
                Debug.Log(callback.action.bindings[1].path);
                Debug.Log(callback.action.bindings[1].overridePath);
                callback.Dispose();
                playerInputAcitons.Player.Enable();
            })
            .Start();
        
    }
}