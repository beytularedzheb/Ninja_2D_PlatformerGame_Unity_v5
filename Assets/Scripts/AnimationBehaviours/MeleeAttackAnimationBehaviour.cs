using UnityEngine;

public class MeleeAttackAnimationBehaviour : StateMachineBehaviour
{
    PlayerController instance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("Player"))
        {
            instance = PlayerController.GetInstance;

            instance.isAttacking = true;

            if (instance.grounded)
            {
                instance.rigidBody.velocity = Vector2.zero;
                instance.isAttackingInAir = false;
            }
            else
            {
                instance.isAttackingInAir = true;
            }
        }
        else if (animator.gameObject.CompareTag("Enemy"))
        {
            animator.GetComponent<Character>().Attack = true;
            animator.SetFloat("speed", 0);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.CompareTag("Player") && instance.isAttackingInAir && instance.grounded)
        {
            instance.rigidBody.velocity = Vector2.zero;
            instance.isAttackingInAir = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("Player"))
        {
            instance.isAttacking = false;
            instance.isAttackingInAir = false;
        }
        else if (animator.gameObject.CompareTag("Enemy"))
        {
            animator.GetComponent<Character>().Attack = false;
            animator.ResetTrigger("attack");
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
