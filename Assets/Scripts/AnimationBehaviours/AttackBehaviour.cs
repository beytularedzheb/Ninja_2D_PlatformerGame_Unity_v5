using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Attack = true;
        animator.SetFloat("speed", 0);

        if (animator.tag.Equals("Player"))
        {
            if (Player.Instance.OnGround)
            {
                animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Attack = false;

        // to disable the sword collider
        if (stateInfo.IsTag("MeleeAttack"))
        {
            animator.GetComponent<Character>().MeleeAttack();
        }
        animator.ResetTrigger("attack");
        animator.ResetTrigger("throw");
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
