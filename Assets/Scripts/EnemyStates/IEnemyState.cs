using UnityEngine;

public interface IEnemyState
{
    // like void Update()
    void Execute();
    //like void Start()
    void Enter(Enemy enemy);
    void Exit();
    void OnTriggerEnter(Collider2D other);
}
