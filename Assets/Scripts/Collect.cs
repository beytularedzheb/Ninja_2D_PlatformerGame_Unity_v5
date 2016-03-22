using UnityEngine;

public class Collect : MonoBehaviour
{
    public int points;
    public bool destroyParent;
    [SerializeField]
    AudioClip ac;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ac != null)
            {
                AudioSource.PlayClipAtPoint(ac, transform.position, 0.5f);
            }
            if (destroyParent && transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }       
    }
}
