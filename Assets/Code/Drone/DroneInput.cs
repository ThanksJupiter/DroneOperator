using UnityEngine;
using UnityEngine.InputSystem;

public class DroneInput : MonoBehaviour, Drone_Actions.IControlActions
{
    public Drone_Actions inputActions;

    public Vector2 look { get; private set; }
    public Vector3 move { get; private set; }

    public bool shoot { get; set; }

    void Start()
    {
        inputActions = new Drone_Actions();
        inputActions.Control.SetCallbacks(this);
        inputActions.Control.Enable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector3>();
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
}
