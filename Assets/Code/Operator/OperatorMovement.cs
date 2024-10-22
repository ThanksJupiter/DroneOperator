using UnityEngine;

namespace Operator
{
    public class OperatorMovement : MonoBehaviour
    {
        public Vector3 moveInputVector;
        public Vector3 lookInputVector;

        public float activeMoveSpeed = 1f;

        public float rotationSharpness = 10f;
    }
}