using Drone;
using KinematicCharacterController;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static UnityEngine.GraphicsBuffer;

namespace Operator
{
    [System.Serializable]
    public class OperatorContext
    {
        [Get] public KinematicCharacterMotor motor;
        [Get] public OperatorMovement movement;
        [Get] public OperatorAnimatorBridge animator;
        [Get] public OperatorInput input;
        [Get] public AimLaser laser;
        public OperatorSettings settings;
        public DroneCamera camera;
        [Get] public Transform transform;
        public DroneMachine drone;
        public ParticleSystem shootParticleSystem;
        public Transform firePoint;
        public GameObject hitEffectPrefab;
        public GameObject hitWallEffectPrefab;
        public RectTransform lockOn;
        public ITarget lockOnTarget = null;
        public Light muzzleFlash;
    }

    public class OperatorMachine : StateMachine<OperatorContext>, ICharacterController
    {
        private void Start()
        {
            context.motor.CharacterController = this;
            context.movement.activeMoveSpeed = context.settings.moveSpeed;
            context.muzzleFlash.enabled = false;
            ActivateState<OperatorDefaultState>();
        }

        protected override void PreUpdate()
        {
            SetInput();
            context.laser.UpdatePosition();
        }

        private void SetInput()
        {
            Vector3 moveInputVector = new Vector3(context.input.move.x, 0f, context.input.move.y);

            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(context.camera.transform.rotation * Vector3.forward, Vector3.up).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(context.camera.transform.rotation * Vector3.up, Vector3.up).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Vector3.up);
            context.movement.moveInputVector = cameraPlanarRotation * moveInputVector;

            Vector3 aimInputVector = new Vector3(context.input.aim.x, 0f, context.input.aim.y);

            context.movement.lookInputVector = context.input.aim.sqrMagnitude > 0f ? cameraPlanarRotation * aimInputVector : context.movement.moveInputVector;
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
            OperatorMovement movement = context.movement;
            if (movement.lookInputVector.sqrMagnitude > 0f)
            {
                float orientationSharpness = context.movement.rotationSharpness;
                float effectedOrientationSharpness = orientationSharpness;

                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(context.motor.CharacterForward, movement.lookInputVector, 1 - Mathf.Exp(-effectedOrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, context.motor.CharacterUp);
            }

            if (context.input.targetLock)
            {
                // TODO make sophisticated lock-on system
                float closestTargetDistance = float.MaxValue;
                List<ITarget> lockOnTargets = new List<ITarget>();
                Collider[] overlappedColliders = Physics.OverlapSphere(context.transform.position, context.settings.lockOnRadius);
                for (int i = 0; i < overlappedColliders.Length; i++)
                {
                    if (overlappedColliders[i].TryGetComponent(out ITarget target))
                    {
                        // Line of sight
                        Vector3 directionToTarget = target.WorldPosition - context.transform.position;
                        directionToTarget.y = 0f;

                        bool lineOfSightBlocked = Physics.Raycast(transform.position + Vector3.up, directionToTarget, out RaycastHit hit);

                        lineOfSightBlocked = lineOfSightBlocked && !hit.transform.TryGetComponent(out ITarget losCheckTarget);
                        Debug.DrawRay(transform.position + Vector3.up, directionToTarget, lineOfSightBlocked? Color.red : Color.green);
                        if (lineOfSightBlocked)
                            continue;

                        if (!lockOnTargets.Contains(target))
                        {
                            lockOnTargets.Add(target);
                            float distance = Vector3.Distance(context.transform.position, target.WorldPosition);
                            if (distance <= closestTargetDistance)
                            {
                                closestTargetDistance = distance;
                                context.lockOnTarget = target;
                            }
                        }
                    }
                }

                if (lockOnTargets.Count == 0)
                {
                    context.lockOnTarget = null;
                    context.lockOn.gameObject.SetActive(false);
                }

                if (context.lockOnTarget != null)
                {
                    Vector3 directionToClosestTarget = context.lockOnTarget.WorldPosition - context.transform.position;
                    directionToClosestTarget.y = 0f;
                    Quaternion rotationToClosestTarget = Quaternion.LookRotation(directionToClosestTarget, context.motor.CharacterUp);
                    currentRotation = rotationToClosestTarget;

                    Vector3 lockOnPosition = context.lockOnTarget.WorldPosition;
                    lockOnPosition.y = context.firePoint.transform.position.y;
                    context.lockOn.position = context.camera.camera.WorldToScreenPoint(lockOnPosition);
                    context.lockOn.gameObject.SetActive(true);
                    return;
                }
            }

            if (context.lockOnTarget != null || context.lockOn.gameObject.activeSelf)
            {
                context.lockOnTarget = null;
                context.lockOn.gameObject.SetActive(false);
            }

            Vector3 currentUp = (currentRotation * Vector3.up);
            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-50f * deltaTime));
            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!context.motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity += Vector3.down * context.settings.gravity * deltaTime;
                return;
            }

            Vector3 inputRight = Vector3.Cross(context.movement.moveInputVector, context.motor.CharacterUp);
            Vector3 effectiveGroundNormal = context.motor.GroundingStatus.GroundNormal;
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * context.movement.moveInputVector.magnitude;

            float stableMoveSpeed = context.movement.activeMoveSpeed;

            Vector3 targetMovementVelocity = reorientedInput * stableMoveSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-context.settings.movementSharpness * deltaTime));
        }
    }
}