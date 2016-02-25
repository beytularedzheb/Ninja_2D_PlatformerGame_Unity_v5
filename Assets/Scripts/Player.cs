using UnityEngine;

public class Player : Character
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
    private bool airControl;

    private Vector2 startPosition;

    public Rigidbody2D Rigidbody { get; set; }
    
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

    public override void Start()
    {
        base.Start();
        startPosition = transform.position;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y <= -14f)
        {
            Rigidbody.velocity = Vector2.zero;
            transform.position = startPosition;
        }
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
            CharacterAnimator.SetBool("land", true);
        }
        if (!Attack && !Slide && (OnGround || airControl))
        {
            Rigidbody.velocity = new Vector2(movementAxises.x * movementSpeed, Rigidbody.velocity.y);
        }
        if (Jump && Rigidbody.velocity.y == 0)
        {
            Rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        CharacterAnimator.SetFloat("speed", Mathf.Abs(movementAxises.x));
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
            ChangeDirection();
        }
    }

    private void HandleInput()
    {
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterAnimator.SetTrigger("jump");
        }
        // melee attack
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CharacterAnimator.SetTrigger("attack");
        }
        // slide
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CharacterAnimator.SetTrigger("slide");
        }
        // throw knife
        if (Input.GetKeyDown(KeyCode.X))
        {
            CharacterAnimator.SetTrigger("throw");
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
                    // consider about Sight and Enemy !!!!!!!!!!!!!!!!!
                    if (!colliders[i].gameObject.name.Equals("Sight") && 
                        !colliders[i].gameObject.tag.Equals("Enemy") && 
                        colliders[i].gameObject != gameObject)
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
            CharacterAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            CharacterAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowKnife(int inTheAir)
    {
        // inTheAir is integer because animation events do not work with boolean parameters
        if (!OnGround && inTheAir == 1 || OnGround && inTheAir == 0)
        {
            base.ThrowKnife(inTheAir);
        }
    }
}
