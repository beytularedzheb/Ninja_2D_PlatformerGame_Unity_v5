using System;
using System.Collections;
using UnityEngine;

/* to inform the enemy that the player is dead and there 
   is no need attacking him anymore */
public delegate void DeadEventHandler();

public class Player : Character
{
    public event DeadEventHandler Dead;

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

    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

    private bool immortal = false;
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

    public override bool IsDead
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

    public override void Start()
    {
        base.Start();

        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            if (transform.position.y <= -14f)
            {
                Death();
            }
            HandleInput();
        }
    }

	void FixedUpdate()
    {
        if (!TakingDamage && !IsDead)
        {
            Vector2 movementInputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            OnGround = IsGrounded();
            HandleLayers();
            HandleMovement(movementInputVector);
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

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            // change 10 to variable which value is according to the weapon (knife or sword)
            health -= 10;

            if (!IsDead)
            {
                CharacterAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                Rigidbody.velocity = Vector2.zero;
                CharacterAnimator.SetLayerWeight(1, 0);
                CharacterAnimator.SetTrigger("die");
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);  
    }

    public override void Death()
    {
        Rigidbody.velocity = Vector2.zero;
        CharacterAnimator.SetTrigger("idle");
        health = 30; // make a variable
        transform.position = startPosition;
    }
}
