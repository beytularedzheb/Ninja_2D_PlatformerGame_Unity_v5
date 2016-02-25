using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;
    private float throwTimer;
    private float throwCoolDown = 3f; // in seconds
    private bool canThrow = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
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

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
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
