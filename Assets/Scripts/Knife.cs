using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour
{
    [SerializeField]
    private float speed;

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

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
