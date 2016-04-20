using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected int health;

    [SerializeField]
    private List<string> damageSources;

    private Animator animator;
    protected bool facingRight;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public abstract bool IsDead { get; }

    public Animator CharacterAnimator
    {
        get
        {
            return animator;
        }

        private set
        {
            animator = value;
        }
    }

    public List<string> DamageSources
    {
        get
        {
            return damageSources;
        }
    }

    public abstract IEnumerator TakeDamage();

    public virtual void Start()
    {
        facingRight = true;
        CharacterAnimator = GetComponent<Animator>();
    }

    public abstract void Update();

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (DamageSources.Contains(other.tag))
        {
            // see more about coroutines: http://docs.unity3d.com/Manual/Coroutines.html
            StartCoroutine(TakeDamage());
        }
    }
}
