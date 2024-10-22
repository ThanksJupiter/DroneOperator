using UnityEngine;
using Utils;

namespace Operator
{
    public class OperatorAnimatorBridge : MonoBehaviour
    {
        [Get] private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void UpdateInputMove(Vector2 inputMove, float dampTime = -1.0f)
        {
            if (dampTime > 0.0f)
            {
                animator.SetFloat(Param.InputMoveX, inputMove.x, dampTime, Time.deltaTime);
                animator.SetFloat(Param.InputMoveY, inputMove.y, dampTime, Time.deltaTime);
                return;
            }

            animator.SetFloat(Param.InputMoveX, inputMove.x);
            animator.SetFloat(Param.InputMoveY, inputMove.y);
        }

        public static class Param
        {
            public static readonly int Empty = Animator.StringToHash("Empty");
            public static readonly int InputMoveX = Animator.StringToHash("Input.MoveX");
            public static readonly int InputMoveY = Animator.StringToHash("Input.MoveY");
        }
    }
}