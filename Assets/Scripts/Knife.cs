using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rigidbody2d;

    private Vector2 direction;

	void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate()
    {
        rigidbody2d.velocity = direction * speed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
