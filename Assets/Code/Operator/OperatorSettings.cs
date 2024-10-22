using UnityEngine;

[CreateAssetMenu(fileName = "OperatorSettings", menuName = "Scriptable Objects/OperatorSettings")]
public class OperatorSettings : ScriptableObject
{
    public float moveSpeed = 5f;
    public float movementSharpness = 10f;
    public float gravity = 20f;
}
