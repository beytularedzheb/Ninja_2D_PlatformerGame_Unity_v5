using UnityEngine;

public class IdleState : EnemyState
{
    private float idleTimer;
    private float idleDuration;

    public override void Enter(Enemy enemy)
    {
        idleDuration = UnityEngine.Random.Range(1, 10);
        this.enemy = enemy;
    }

    public override void Execute()
    {
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
        Idle();
    }

    public override void Exit()
    {

    }

    public override void OnTriggerEnter(Collider2D other)
    {
        if (enemy.DamageSources.Contains(other.tag))
        {
            enemy.Target = PlayerController.GetInstance.gameObject;
        }
    }

    private void Idle()
    {
        enemy.CharacterAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
