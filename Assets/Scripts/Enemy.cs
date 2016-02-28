using System;
using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float throwRange;

    private IEnemyState currentState;
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

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
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

        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
	}

    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }

            LookAtTarget();
        }
    }

    public void ChangeState(IEnemyState newState)
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
        if (!Attack)
        {
            CharacterAnimator.SetFloat("speed", 1);

            /* multiply by Time.deltaTime because we want enemy to move
               with the same speed on different devices (it depends on the FPS rate) */
            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
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

            if (xDirection < 0 && facingRight || xDirection > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void RemoveTarget()
    {
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

    public override void Death()
    {
        Destroy(gameObject);
    }
}
