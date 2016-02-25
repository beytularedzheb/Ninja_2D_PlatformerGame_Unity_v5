using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected Transform knifePosition;

    [SerializeField]
    private GameObject knifePrefab;

    [SerializeField]
    protected float movementSpeed;

    private Animator animator;
    protected bool facingRight;

    public bool Attack { get; set; }

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

    // Use this for initialization
    public virtual void Start()
    {
        facingRight = true;
        CharacterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
	
	}

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public virtual void ThrowKnife(int inTheAir)
    {
        // inTheAir is integer because animation events do not work with boolean parameters

        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

}
