using UnityEngine;

public class MeleeState : EnemyState
{
    private float attackTimer;
    private float attackCoolDown;
    private bool canAttack = true;
    
    public override void Enter(Enemy enemy)
    {
        attackCoolDown = UnityEngine.Random.Range(1, 4);
        this.enemy = enemy;
    }

    public override void Execute()
    {
        Attack();
        if (!enemy.InMeleeRange)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public override void Exit()
    {

    }

    public override void OnTriggerEnter(Collider2D other)
    {

    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }
        if (canAttack)
        {
            canAttack = false;
            enemy.CharacterAnimator.SetTrigger("attack");
        }
    }
}
