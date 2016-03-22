using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TransportPlayerOnMovablePlatform : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 offset;
    private Vector2 boxCastSize;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        offset = new Vector3(boxCollider.offset.x, (boxCollider.size.y / 2 - boxCollider.offset.y), 0);
        boxCastSize = new Vector2(boxCollider.size.x - 0.05f, 0.001f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool isPlayerOnMe = Physics2D.BoxCast(transform.position + offset, boxCastSize, 0, Vector2.up, 0.1f, 1 << LayerMask.NameToLayer("Player"));

            if (transform.parent.gameObject.CompareTag("MoveablePlatform") && isPlayerOnMe)
            {
                other.transform.parent = transform;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.transform.parent == transform)
            {
                other.transform.parent = PlayerController.GetInstance.DefaultParent;
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
