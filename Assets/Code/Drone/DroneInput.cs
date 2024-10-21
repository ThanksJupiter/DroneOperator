using UnityEngine;
using UnityEngine.InputSystem;

public class DroneInput : MonoBehaviour, Drone_Actions.IControlActions
{
    public Drone_Actions droneActions;

    public Vector2 look { get; private set; }
    public Vector2 move { get; private set; }

    void Start()
    {
        droneActions = new Drone_Actions();
        droneActions.Control.SetCallbacks(this);
        droneActions.Control.Enable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
}
