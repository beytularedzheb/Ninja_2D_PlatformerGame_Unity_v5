using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private LayerMask destroyIfTouchingLayers;

    private Rigidbody2D rigidBody;

    public Vector2 Direction { get; set; }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate()
    {
        rigidBody.velocity = Direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((destroyIfTouchingLayers & (1 << other.gameObject.layer)) != 0)
        {
            if (other.CompareTag("Enemy") && other.gameObject.GetComponent<Enemy>().IsDead)
            {
                return;
            }
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
