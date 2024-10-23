using KinematicCharacterController;
using Operator;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utils;

namespace Zombie
{
    [System.Serializable]
    public class ZombieContext
    {
        [Get] public KinematicCharacterMotor motor;
        [Get] public ZombieAnimatorBridge animatorBridge;
        [Get] public TargetDummy targetDummy;
        [Get] public Transform transform;
        public float discoverRange = 11f;
        public OperatorMachine target;
        public float attackRange = 1f;
        public float moveSpeed = 5f;
        public Vector3 moveInputVector = Vector3.zero;
        public bool killedPlayer = false;
    }

    public class ZombieMachine : StateMachine<ZombieContext>, ICharacterController
    {
        private void Start()
        {
            context.motor.CharacterController = this;
            ActivateState<ZombieDefaultState>();
        }

        protected override void PreUpdate()
        {
            if (context.targetDummy.IsDead)
            {
                ActivateState<ZombieDeadState>();
                return;
            }
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
            if (context.targetDummy.IsDead)
                return;

            if (context.target == null)
                return;

            Vector3 directionToTarget = context.target.transform.position - context.transform.position;
            directionToTarget.y = 0f;
            currentRotation = Quaternion.LookRotation(directionToTarget, context.motor.CharacterUp);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (context.targetDummy.IsDead)
            {
                currentVelocity = Vector3.zero;
                return;
            }

            if (context.target == null)
            {
                currentVelocity = Vector3.zero;
                return;
            }

            currentVelocity = context.moveInputVector * context.moveSpeed;
        }
    }
}