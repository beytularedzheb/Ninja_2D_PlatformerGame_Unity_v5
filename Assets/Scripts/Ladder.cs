using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private PlatformEffector2D upperPlatform;

    private float gravityScale;

    private PlayerController player;

    void Start()
    {
        if (null == upperPlatform)
        {
            Debug.LogException(new System.NullReferenceException("\"platformEffectorUpperPlatform\" is not assigned!"));
        }

        player = PlayerController.GetInstance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // remove Player from colliderMask
            upperPlatform.colliderMask &= ~(1 << LayerMask.NameToLayer("Player"));

            player.canClimb = true;
            gravityScale = player.rigidBody.gravityScale;
            player.rigidBody.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // add Player from colliderMask
            upperPlatform.colliderMask |= (1 << LayerMask.NameToLayer("Player"));

            player.canClimb = false;
            player.rigidBody.gravityScale = gravityScale;
            player.anim.enabled = true;
            player.anim.SetBool("climb", false);
        }
    }
}
