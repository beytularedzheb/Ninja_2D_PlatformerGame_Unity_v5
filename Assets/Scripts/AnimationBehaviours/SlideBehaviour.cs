using UnityEngine;

public class SlideBehaviour : StateMachineBehaviour
{
    private Vector2 originalSize;
    private Vector2 originalOffset;

    private Vector2 slideSize = new Vector2(1.35f, 1.94f);
    private Vector2 slideOffset = new Vector2(0, -1.23f);

    private BoxCollider2D characterBoxCollider;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Slide = true;
        if (characterBoxCollider == null)
        {
            characterBoxCollider = Player.Instance.GetComponent<BoxCollider2D>();
            originalSize = characterBoxCollider.size;
            originalOffset = characterBoxCollider.offset;
        }

        characterBoxCollider.size = slideSize;
        characterBoxCollider.offset = slideOffset;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Slide = false;
        animator.ResetTrigger("slide");

        characterBoxCollider.size = originalSize;
        characterBoxCollider.offset = originalOffset;
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
