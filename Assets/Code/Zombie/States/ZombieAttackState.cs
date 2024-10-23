using UnityEngine;

namespace Zombie
{
    public class ZombieAttackState : ZombieMachine.UpdateState
    {
        float lilTimer = 0f;

        public override void OnEnter()
        {
            context.animatorBridge.ActionAttack = true;
            context.moveInputVector = Vector3.zero;

            lilTimer = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            lilTimer += deltaTime;
            if (lilTimer >= .35f)
            {
                context.target.context.animator.ActionDead = true;
                context.target.InvokeReloadScene();
                context.killedPlayer = true;
                machine.ActivateState<ZombieDefaultState>();
            }
        }

        public override void OnLeave()
        {
            context.animatorBridge.ActionAttack = false;
        }
    }
}