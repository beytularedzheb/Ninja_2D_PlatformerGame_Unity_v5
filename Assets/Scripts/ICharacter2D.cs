using System.Collections;
using UnityEngine;

public interface ICharacter2D
{
    void Move(Vector2 velocity);

    void Throw();

    void ChangeDirection();

    IEnumerator TakeDamage();

    //void Die();*/
}
