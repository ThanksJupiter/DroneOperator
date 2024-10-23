using UnityEngine;

namespace Zombie
{
    public class ZombieDeadState : ZombieMachine.UpdateState
    {
        public override void OnEnter()
        {
            context.moveInputVector = Vector3.zero;
            context.animatorBridge.ActionDead = true;
        }
    }
}