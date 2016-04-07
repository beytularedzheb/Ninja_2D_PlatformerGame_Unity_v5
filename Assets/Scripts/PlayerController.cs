using UnityEngine;

/* to inform the enemy that the player is dead and there 
   is no need attacking him anymore */
public delegate void DeadEventHandler();

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour, ICharacter2D
{
    #region Fields

    public event DeadEventHandler Dead;

    public int health;

    public Transform DefaultParent { get; private set; }

    public readonly Vector2 slidingOffset = new Vector2(-0.03342819f, -1.108189f);
    public readonly Vector2 slidingSize = new Vector2(1.584584f, 2.135715f);

    [HideInInspector]
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public Animator anim;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private GameObject knifePrefab;
    private Transform knifePosition;

    [SerializeField]
    private Vector2 speed;

    private Vector2 inputVelocity;

    private bool facingRight = true;
    public bool grounded = false;
    private bool jump = false;
    [SerializeField]
    private float jumpVelocity;

    [HideInInspector]
    public bool isSliding = false;
    [HideInInspector]
    public bool canClimb = false;
    [HideInInspector]
    public bool isMeleeAttacking = false;
    [HideInInspector]
    public bool isThrowingOnGround = false;
    [HideInInspector]
    public bool isThrowingInAir = false;

    private float velocityXSmoothing;
    private float accelerationTimeAirborne = 0.005f;
    private float accelerationTimeGrounded = 0f;

    private Vector3 offset;
    private Vector2 boxCastSize;

    // Singleton
    private static PlayerController instance;
    public static PlayerController GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerController>();
            }

            return instance;
        }
    }

    #endregion

    private void Start()
    {
        DefaultParent = transform.parent;

        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        knifePosition = transform.FindChild("knifePosition");

        if (null == knifePosition)
        {
            Debug.LogException(new System.NullReferenceException("\"knifePosition\" child is missing!"));
        }

        offset = new Vector3(boxCollider.offset.x, -(boxCollider.size.y / 2 - boxCollider.offset.y), 0);
        boxCastSize = new Vector2(boxCollider.size.x - 0.05f, 0.001f);
    }
    
    private void Update()
    {
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // Gizmos.DrawCube(transform.position + offset, boxCastSize);
        grounded = Physics2D.BoxCast(transform.position + offset, boxCastSize, 0, Vector2.down, 0.1f, whatIsGround);

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
        if (!canClimb) // !anim.GetCurrentAnimatorStateInfo(0).IsName("Climb")
        {
            if (Input.GetButtonDown("Fire1") && !isMeleeAttacking)
            {
                anim.SetTrigger("attack");
            }
            if (Input.GetButtonDown("Fire2") && !isThrowingOnGround && !isThrowingInAir)
            {
                anim.SetTrigger("throw");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) &&
                grounded &&
                anim.GetCurrentAnimatorStateInfo(0).IsTag("Run"))
            {
                anim.SetTrigger("slide");
            }
        }
    }
    
    private void FixedUpdate()
    {			
        if (!isSliding)
        {
            anim.SetFloat("speed", Mathf.Abs(inputVelocity.x));

            if (isThrowingInAir || (!isMeleeAttacking && !isThrowingOnGround))
            {
                Move(inputVelocity);
                Jump();
            }

            HandleAnimationLayers();
        }
    }

    public void Jump()
    {
        // If the player should jump...
        if (jump)
        {
            anim.SetTrigger("jump");
            // Add a vertical force to the player.
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpVelocity);
            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
        else if (rigidBody.velocity.y < 0 && !anim.GetBool("land"))
        {
            anim.SetBool("land", true);
        }
    }

    public void Move(Vector2 velocity)
    {
        // when you need to move the player vertically consider about 
        Vector2 targetVelocity = new Vector2(velocity.x * speed.x, rigidBody.velocity.y);
        targetVelocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocityXSmoothing, (grounded) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if (canClimb)
        {
            anim.enabled = rigidBody.velocity.y != 0 || !anim.GetCurrentAnimatorStateInfo(0).IsName("Climb");
            anim.SetBool("climb", !grounded && anim.enabled);
            
            targetVelocity.y = velocity.y * speed.y;

            if (grounded)
            {
                anim.enabled = true;
                Flip(velocity);
            }   
        }
        else
        {
            if (!isThrowingInAir)
            {
                Flip(velocity);
            }
        }

        rigidBody.velocity = targetVelocity;
    }

    public void ChangeDirection()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Throw()
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Knife>().Direction = Vector2.right;
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Knife>().Direction = Vector2.left;
        }
    }

    private void Flip(Vector2 velocity)
    {
        if ((facingRight && velocity.x < 0) || (!facingRight && velocity.x > 0))
        {
            offset.x *= -1;
            ChangeDirection();
        }
    }

    private void HandleAnimationLayers()
    {
        if (!grounded && !canClimb)
        {
            anim.SetLayerWeight(1, 1);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }
    }

    public bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }

            return health <= 0;
        }
    }

    public void OnDead()
    {
        // if there is any instance
        if (Dead != null)
        {
            // then execute it
            Dead();
        }
    }
}
