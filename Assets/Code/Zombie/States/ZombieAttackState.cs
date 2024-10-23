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
            Debug.Log("eat player tbh :)");
            context.target.context.animator.ActionDead = true;
            context.killedPlayer = true;
            context.transform.GetComponent<ZombieMachine>().Invoke("ReloadScene", 3f);
        }

        public override void OnUpdate(float deltaTime)
        {
            lilTimer += deltaTime;
            if (lilTimer >= .1f)
            {
                machine.ActivateState<ZombieDefaultState>();
            }
        }

        public override void OnLeave()
        {
            context.animatorBridge.ActionAttack = false;
        }

        private void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}