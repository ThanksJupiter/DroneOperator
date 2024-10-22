using UnityEngine;

namespace Operator
{
    public class DefaultOperatorState : OperatorMachine.UpdateState
    {
        public override void OnUpdate(float deltaTime)
        {
            Vector3 xzMove = new Vector3(context.movement.moveInputVector.x, 0f, context.movement.moveInputVector.z);
            Debug.DrawRay(context.transform.position + Vector3.up * .1f, xzMove, Color.red);
            float xDot = Vector3.Dot(context.motor.CharacterRight, xzMove);
            float yDot = Vector3.Dot(context.motor.CharacterForward, xzMove);

            Vector2 vector = new Vector2(xDot, yDot);
            context.animator.UpdateInputMove(vector, 1.0f / context.settings.movementSharpness);
        }
    }
}