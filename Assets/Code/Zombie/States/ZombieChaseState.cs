using UnityEngine;

namespace Zombie
{
    public class ZombieChaseState : ZombieMachine.UpdateState
    {
        public override void OnEnter()
        {
            context.animatorBridge.ActionChase = true;
        }

        public override void OnLeave()
        {
            context.animatorBridge.ActionChase = false;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (context.target == null)
            {
                machine.ActivateState<ZombieDefaultState>();
                return;
            }

            Vector3 directionToTarget = context.target.transform.position - context.transform.position;
            directionToTarget.y = 0f;
            context.moveInputVector = directionToTarget.normalized;

            float distance = Vector3.Distance(context.transform.position, context.target.transform.position);
            if (distance <= context.attackRange)
            {
                machine.ActivateState<ZombieAttackState>();
                return;
            }
        }
    }
}