using UnityEngine;

[CreateAssetMenu(fileName = "OperatorSettings", menuName = "Scriptable Objects/OperatorSettings")]
public class OperatorSettings : ScriptableObject
{
    public float moveSpeed = 5f;
    public float shootingMoveSpeed = .1f;
    public float movementSharpness = 10f;
    public float gravity = 20f;

    [Header("Shooting")]
    public float fireRate = .1f;
    public float fireCastRadius = .1f;
    public float lockOnRadius = 10f;
}
