using UnityEngine;

namespace Operator
{
    public class OperatorShootState : OperatorMachine.UpdateState
    {
        private float fireTimer = 0f;

        public override void OnEnter()
        {
            context.movement.activeMoveSpeed = context.settings.shootingMoveSpeed;
            context.animator.InputShoot = true;
            context.shootParticleSystem.Play();
            fireTimer = 0f;
        }

        public override void OnLeave()
        {
            context.animator.InputShoot = false;
            context.shootParticleSystem.Stop();
        }

        public override void OnUpdate(float deltaTime)
        {
            Vector3 xzMove = new Vector3(context.movement.moveInputVector.x, 0f, context.movement.moveInputVector.z);
            float xDot = Vector3.Dot(context.motor.CharacterRight, xzMove);
            float yDot = Vector3.Dot(context.motor.CharacterForward, xzMove);

            Vector2 vector = new Vector2(xDot, yDot);
            context.animator.UpdateInputMove(vector * context.settings.shootingMoveSpeed, 1.0f / context.settings.movementSharpness);

            fireTimer += deltaTime;

            if (fireTimer >= context.settings.fireRate)
            {
                fireTimer = 0f;

                Vector3 fireDirection = context.lockOnTarget != null ? context.lockOnTarget.WorldPosition - context.firePoint.position : context.firePoint.forward;
                Ray ray = new Ray(context.firePoint.position, fireDirection);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    if (hit.transform.TryGetComponent(out ITarget target))
                    {
                        target.Hit();

                        if (target.IsDead)
                            context.lockOnTarget = null;
                        GameObject.Instantiate(context.hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
                    }
                    else
                    {
                        Vector3 effectDirection = Vector3.Reflect(fireDirection, hit.normal);
                        GameObject.Instantiate(context.hitWallEffectPrefab, hit.point, Quaternion.LookRotation(effectDirection, Vector3.up));
                    }
                }
            }

            if (!context.input.shoot)
            {
                machine.ActivateState<OperatorDefaultState>();
                return;
            }
        }
    }
}