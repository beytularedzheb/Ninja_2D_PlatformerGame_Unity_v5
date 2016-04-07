using UnityEngine;

public class RangedState : EnemyState
{
    private float throwTimer;
    private float throwCoolDown;
    private bool canThrow = true;

    public override void Enter(Enemy enemy)
    {
        throwCoolDown = UnityEngine.Random.Range(1, 5);
        this.enemy = enemy;
    }

    public override void Execute()
    {
        ThrowKnife();

        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }

        if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public override void Exit()
    {

    }

    public override void OnTriggerEnter(Collider2D other)
    {

    }

    private void ThrowKnife()
    {
        throwTimer += Time.deltaTime;

        if (throwTimer >= throwCoolDown)
        {
            canThrow = true;
            throwTimer = 0;
        }
        if (canThrow)
        {
            canThrow = false;
            enemy.CharacterAnimator.SetTrigger("throw");
        }
    }
}
