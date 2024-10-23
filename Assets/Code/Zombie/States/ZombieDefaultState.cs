using Operator;
using UnityEngine;

namespace Zombie
{
    public class ZombieDefaultState : ZombieMachine.UpdateState
    {
        public override void OnUpdate(float deltaTime)
        {
            if (context.killedPlayer)
                return;

            Collider[] overlappedColliders = Physics.OverlapSphere(context.transform.position, context.discoverRange);
            for (int i = 0; i < overlappedColliders.Length; i++)
            {
                if (overlappedColliders[i].TryGetComponent(out OperatorMachine operatorMachine))
                {
                    // start chasin'

                    Vector3 directionToTarget = operatorMachine.transform.position - context.transform.position;
                    directionToTarget.y = 0f;
                    if (Physics.Raycast(context.transform.position + Vector3.up, directionToTarget, out RaycastHit hit))
                    {
                        if (!hit.transform.TryGetComponent(out OperatorMachine rayHitOperatorMachine))
                            return;
                    }

                    context.target = operatorMachine;
                    machine.ActivateState<ZombieChaseState>();
                }
            }
        }
    }
}