using UnityEngine;

public class ThrowAnimationBehaviour : StateMachineBehaviour
{
    PlayerController instance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("Player"))
        {
            instance = PlayerController.GetInstance;

            if (instance.grounded)
            {
                instance.isThrowingOnGround = true;
                instance.isThrowingInAir = false;
                instance.rigidBody.velocity = Vector2.zero;
            }
            else
            {
                instance.isThrowingOnGround = false;
                instance.isThrowingInAir = true;
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("Player"))
        {
            instance.isThrowingOnGround = false;
            instance.isThrowingInAir = false;
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
