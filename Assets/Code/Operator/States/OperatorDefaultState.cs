using UnityEngine;

namespace Operator
{
    public class OperatorDefaultState : OperatorMachine.UpdateState
    {
        public override void OnEnter()
        {
            context.movement.activeMoveSpeed = context.settings.moveSpeed;

        }

        public override void OnLeave()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            Vector3 xzMove = new Vector3(context.movement.moveInputVector.x, 0f, context.movement.moveInputVector.z);
            float xDot = Vector3.Dot(context.motor.CharacterRight, xzMove);
            float yDot = Vector3.Dot(context.motor.CharacterForward, xzMove);

            Vector2 vector = new Vector2(xDot, yDot);
            context.animator.UpdateInputMove(vector, 1.0f / context.settings.movementSharpness);

            if (context.input.shoot)
            {
                machine.ActivateState<OperatorShootState>();
                return;
            }
        }
    }
}