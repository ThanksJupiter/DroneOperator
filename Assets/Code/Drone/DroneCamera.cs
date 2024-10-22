using UnityEngine;

namespace Drone
{
    public class DroneCamera : MonoBehaviour
    {
        public float updatePositionAndRotationSharpness = 10f;
        public float rotationSpeed = 10f;
        public float rotationSharpness = 10f;
        public float minVerticalAngle = -85f;
        public float maxVerticalAngle = 85f;
        public Vector3 PlanarDirection { get; set; }
        public Transform FollowTransform { get; private set; }

        private float targetVerticalAngle = 65f;

        private void Awake()
        {
            PlanarDirection = transform.forward;

            targetVerticalAngle = 0f;
        }

        public void SetFollowTransform(Transform t)
        {
            FollowTransform = t;
        }

        public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
        {
            if (FollowTransform)
            {
                Quaternion rotationFromInput = Quaternion.Euler(Vector3.up * (rotationInput.x * rotationSpeed));
                PlanarDirection = rotationFromInput * PlanarDirection;
                PlanarDirection = Vector3.Cross(Vector3.up, Vector3.Cross(PlanarDirection, Vector3.up));
                Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, Vector3.up);

                targetVerticalAngle -= (rotationInput.y * rotationSpeed);
                targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);
                Quaternion verticalRot = Quaternion.Euler(targetVerticalAngle, 0, 0);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-rotationSharpness * deltaTime));

                transform.rotation = targetRotation;
            }
        }

        public void UpdatePositionAndRotation(Vector3 position, Quaternion rotation, float dt)
        {
            transform.position = Vector3.Lerp(transform.position, position, 1f - Mathf.Exp(-updatePositionAndRotationSharpness * dt));
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f - Mathf.Exp(-updatePositionAndRotationSharpness * dt));
        }
    }
}