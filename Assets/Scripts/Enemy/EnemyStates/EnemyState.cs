using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;

    // like void Update()
    public abstract void Execute();
    //like void Start()
    public abstract void Enter(Enemy enemy);
    public abstract void Exit();
    public abstract void OnTriggerEnter(Collider2D other);
}
