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
        public DroneCamera camera;
    }

    public class DroneMachine : StateMachine<DroneContext>, ICharacterController
    {
        private void Start()
        {
            context.motor.CharacterController = this;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ActivateState<DefaultDroneState>();
        }

        protected override void LateUpdate()
        {
            
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
            
        }
    }
}