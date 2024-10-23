using KinematicCharacterController;
using UnityEngine;
using Utils;

namespace Drone
{
    [System.Serializable]
    public class DroneContext
    {
        [Get] public KinematicCharacterMotor motor;
        [Get] public DroneInput input;
        [Get] public DroneMovement movement;
        public DroneCamera camera;

        public float cameraHeightOffset = .5f;
        public Quaternion cameraRotation = Quaternion.identity;
    }

    public class DroneMachine : StateMachine<DroneContext>, ICharacterController
    {
        private void Start()
        {
            context.motor.CharacterController = this;
            context.camera.SetFollowTransform(transform);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ActivateState<DefaultDroneState>();
        }

        protected override void PreUpdate()
        {
            SetInput();
        }

        private void SetInput()
        {
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(context.camera.transform.rotation * Vector3.forward, Vector3.up).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(context.camera.transform.rotation * Vector3.up, Vector3.up).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Vector3.up);
            context.movement.moveInputVector = cameraPlanarRotation * context.input.move;
        }

        protected override void LateUpdate()
        {
            float dt = Time.deltaTime;

            Vector3 lookInputVector = new Vector3(context.input.look.x, context.input.look.y);
            context.camera.UpdateWithInput(dt, 0f, lookInputVector);
            context.camera.UpdatePositionAndRotation(transform.position + Vector3.down * context.cameraHeightOffset, context.cameraRotation, Time.deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (context.motor.GroundingStatus.FoundAnyGround && context.input.move.y > 0f)
                context.motor.ForceUnground();

            Vector3 targetVelocity = context.movement.moveInputVector * context.movement.moveSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1f - Mathf.Exp(-context.movement.moveSharpness * deltaTime));
        }
    }
}