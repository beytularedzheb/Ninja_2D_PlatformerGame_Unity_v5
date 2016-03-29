using UnityEngine;

public class Collect : MonoBehaviour
{
    public int points;
    public bool destructive;
    [SerializeField]
    private AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (audioClip != null)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position, 0.5f);
            }
            if (destructive)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }       
    }
}
