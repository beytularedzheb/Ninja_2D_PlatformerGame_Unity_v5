using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private GameObject knifePrefab;

    [SerializeField]
    private Transform knifePosition;

    private bool facingRight;
    private Animator animator;

    public Rigidbody2D Rigidbody { get; set; }
    public bool Attack { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    // Singleton
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    void Start()
    {
        facingRight = true;
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
    void Update()
    {
        HandleInput();
    }

	void FixedUpdate()
    {
        Vector2 movementInputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnGround = IsGrounded();
        HandleLayers();
        HandleMovement(movementInputVector);
    }

    private void HandleMovement(Vector2 movementAxises)
    {
        if (Rigidbody.velocity.y < 0)
        {
            animator.SetBool("land", true);
        }
        if (!Attack && !Slide && (OnGround || airControl))
        {
            Rigidbody.velocity = new Vector2(movementAxises.x * movementSpeed, Rigidbody.velocity.y);
        }
        if (Jump && Rigidbody.velocity.y == 0)
        {
            Rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        animator.SetFloat("speed", Mathf.Abs(movementAxises.x));
        if (!Slide && !Attack)
        {
            Flip(movementAxises);
        }
    }

    private void Flip(Vector2 movementAxises)
    {
        if ((movementAxises.x > 0 && !facingRight) || 
            (movementAxises.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            /* flip the player */
            localScale.x *= -1; 
            this.transform.localScale = localScale;
        }
    }

    private void HandleInput()
    {
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("jump");
        }
        // melee attack
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("attack");
        }
        // slide
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetTrigger("slide");
        }
        // throw knife
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("throw");
        }
    }

    private bool IsGrounded()
    {
        // falling down or not moving
        if (Rigidbody.velocity.y <= 0)
        {
            foreach (var point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void ThrowKnife(int inTheAir)
    {
        // inTheAir is integer because animation events do not work with boolean parameters
        if (!OnGround && inTheAir == 1 || OnGround && inTheAir == 0)
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                tmp.GetComponent<Knife>().Initialize(Vector2.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                tmp.GetComponent<Knife>().Initialize(Vector2.left);
            }
        }
    }
}
