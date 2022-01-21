using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IFreeroamActions, GameInput.IMenuActions, GameInput.IDialogActions
{
    //-----------Freeroam events----------------
    public event UnityAction<Vector2> moveEvent;
    public event UnityAction interactEvent;
    public event UnityAction openMenuEvent;

    //-----------Menu events--------------------
    public event UnityAction closeMenuEvent;
    public event UnityAction submitEvent;
    public event UnityAction<Vector2> navigateEvent;

    //-----------Dialog events------------------
    public event UnityAction nextLineEvent;

    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Freeroam.SetCallbacks(this);
            gameInput.Menu.SetCallbacks(this);
            gameInput.Dialog.SetCallbacks(this);
        }

        EnableFreeroamInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    #region Freeroam

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (moveEvent != null)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (interactEvent != null && context.performed)
        {
            interactEvent.Invoke();
        }
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (openMenuEvent != null && context.performed)
        {
            openMenuEvent.Invoke();
        }
    }

    #endregion

    #region Menu

    public void OnCloseMenu(InputAction.CallbackContext context)
    {
        if (closeMenuEvent != null && context.performed)
        {
            closeMenuEvent.Invoke();
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (submitEvent != null && context.performed)
        {
            submitEvent.Invoke();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (navigateEvent != null)
        {
            navigateEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    #endregion

    #region Dialog

    public void OnNextLine(InputAction.CallbackContext context)
    {
        if (nextLineEvent != null && context.performed)
        {
            nextLineEvent.Invoke();
        }
    }

    #endregion

    #region Input Enable/Disable methods

    public void EnableFreeroamInput()
    {
        gameInput.Menu.Disable();
        gameInput.Dialog.Disable();
        gameInput.Freeroam.Enable();
    }

    public void EnableMenuInput()
    {
        gameInput.Freeroam.Disable();
        gameInput.Dialog.Disable();
        gameInput.Menu.Enable();
    }

    public void EnableDialogInput()
    {
        gameInput.Freeroam.Disable();
        gameInput.Menu.Disable();
        gameInput.Dialog.Enable();
    }

    public void DisableAllInput()
    {
        gameInput.Freeroam.Disable();
        gameInput.Menu.Disable();
        gameInput.Dialog.Disable();
    }

    #endregion
}
