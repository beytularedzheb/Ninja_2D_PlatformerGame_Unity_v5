using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.Target = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.Target = null;
        }
    }
}
