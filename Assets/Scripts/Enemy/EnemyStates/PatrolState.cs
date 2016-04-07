using UnityEngine;

public class PatrolState : EnemyState
{
    private float patrolTimer;
    private float patrolDuration;

    public override void Enter(Enemy enemy)
    {
        patrolDuration = UnityEngine.Random.Range(1, 12);
        this.enemy = enemy;
    }

    public override void Execute()
    {
        Patrol();

        enemy.Move();

        if (enemy.Target != null && enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
    }

    public override void Exit()
    {

    }

    public override void OnTriggerEnter(Collider2D other)
    {
        if (other.tag.Equals("Knife"))
        {
            enemy.Target = PlayerController.GetInstance.gameObject;
        }
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
