using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private GameObject upperPlatform;
    private BoxCollider2D[] boxColliderArr;
    private BoxCollider2D enabledCollider;

    private float gravityScale;

    private PlayerController player;

    void Start()
    {
        if (null == upperPlatform)
        {
            Debug.LogException(new System.NullReferenceException("\"upperPlatform\" is not assigned!"));
        }
        else
        {
            boxColliderArr = upperPlatform.GetComponents<BoxCollider2D>();
        }

        player = PlayerController.GetInstance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var boxCollider in boxColliderArr)
            {
                if (boxCollider.enabled)
                {
                    enabledCollider = boxCollider;
                    boxCollider.isTrigger = true;
                }
                else
                {
                    boxCollider.enabled = true;
                }
            }

            player.canClimb = true;
            gravityScale = player.rigidBody.gravityScale;
            player.rigidBody.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var boxCollider in boxColliderArr)
            {
                if (enabledCollider == boxCollider)
                {
                    boxCollider.isTrigger = false;
                }
                else
                {
                    boxCollider.enabled = false;
                }
            }

            player.canClimb = false;
            player.rigidBody.gravityScale = gravityScale;
            player.anim.enabled = true;
            player.anim.SetBool("climb", false);
        }
    }
}
