using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    [SerializeField]
    private float howFarCanSeeAhead;

    [SerializeField]
    private float howFarCanSeeBehind;

    private Vector2 startPosition;
    private EnemyState currentState;
    private bool ignoreTarget;

    public GameObject Target { get; set; }

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void Start()
    {
        base.Start();
        ignoreTarget = false;
        facingRight = false;
        CharacterAnimator.SetTrigger("appear");
        PlayerController.GetInstance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());

        startPosition = transform.position;
    }

    public override void Update()
    {
        if (!IsDead)
        {
            Target = CanSeeTarget();

            currentState.Execute();

            LookAtTarget();
        }
    }

    public GameObject CanSeeTarget()
    {
        Vector2 direction = GetDirection();

        RaycastHit2D hitPlayerAhead = Physics2D.Raycast(
            transform.position,
            direction, howFarCanSeeAhead,
            1 << LayerMask.NameToLayer("Player"));

        if (!ignoreTarget)
        {
            if (hitPlayerAhead)
            {
                return hitPlayerAhead.transform.gameObject;
            }
            else
            {
                RaycastHit2D hitPlayerBehind = Physics2D.Raycast(
                    transform.position,
                    direction * -1, howFarCanSeeBehind,
                    1 << LayerMask.NameToLayer("Player"));

                if (hitPlayerBehind)
                {
                    return hitPlayerBehind.transform.gameObject;
                }
            }
        }
        return null;
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack && !TakingDamage)
        {
            // to fix falling down of enemy from platform
            Vector2 direction = GetDirection();
            if ((direction.x > 0 && transform.position.x < rightEdge.position.x) ||
                (direction.x < 0 && transform.position.x > leftEdge.position.x))
            {
                CharacterAnimator.SetFloat("speed", 1);

                /* multiply by Time.deltaTime because we want enemy to move
                   with the same speed on different devices (it depends on the FPS rate) */
                transform.Translate(direction * (movementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDirection = Target.transform.position.x - transform.position.x;

            if (!Attack && (xDirection < 0 && facingRight || xDirection > 0 && !facingRight))
            {
                ChangeDirection();
            }
        }
    }

    public void RemoveTarget()
    {
        ignoreTarget = true;
        Target = null;
        ChangeState(new PatrolState());
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        // change 10 to variable which value is according to the weapon (knife or sword)
        health -= 10;

        if (!IsDead)
        {            
            CharacterAnimator.SetTrigger("damage");
        }
        else
        {
            CharacterAnimator.SetTrigger("die");
        }

        yield return null;
    }

    public IEnumerator Death()
    {   // "Revival"
        /*CharacterAnimator.ResetTrigger("die");
        CharacterAnimator.SetTrigger("idle");
        health = 30; // variable
        transform.position = startPosition;*/

        // disappears
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
