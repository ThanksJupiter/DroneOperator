using UnityEngine;
using Utils;

namespace Zombie
{
    public class ZombieAnimatorBridge : MonoBehaviour
    {
        [Get] private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public bool ActionChase
        {
            get => animator.GetBool(Param.ActionChase);
            set => animator.SetBool(Param.ActionChase, value);
        }

        public bool ActionAttack
        {
            get => animator.GetBool(Param.ActionAttack);
            set => animator.SetBool(Param.ActionAttack, value);
        }

        public bool ActionDead
        {
            get => animator.GetBool(Param.ActionDead);
            set => animator.SetBool(Param.ActionDead, value);
        }

        public static class Param
        {
            public static readonly int ActionChase = Animator.StringToHash("Action.Chase");
            public static readonly int ActionAttack = Animator.StringToHash("Action.Attack");
            public static readonly int ActionDead = Animator.StringToHash("Action.Dead");
        }
    }
}