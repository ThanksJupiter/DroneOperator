using UnityEngine;
using UnityEngine.InputSystem;

public class OperatorInput : MonoBehaviour, Operator_Actions.IControlActions
{
    public Operator_Actions inputActions;

    public Vector2 aim { get; private set; }
    public Vector2 move { get; private set; }

    public bool shoot { get; private set; }
    public bool targetLock { get; private set; }

    private void Start()
    {
        inputActions = new Operator_Actions();
        inputActions.Control.SetCallbacks(this);
        inputActions.Enable();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shoot = true;
        }
        else if (context.canceled)
        {
            shoot = false;
        }
    }

    public void OnTargetLock(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            targetLock = true;
        }
        else if (context.canceled)
        {
            targetLock = false;
        }
    }
}
