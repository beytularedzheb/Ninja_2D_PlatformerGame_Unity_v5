using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    private List<Collider2D> others;

	void Awake()
    {
        foreach (var other in others)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
        }  
	}
}
