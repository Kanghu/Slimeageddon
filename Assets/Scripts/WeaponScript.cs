using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public Animator anim;
    public Collider2D player;

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreCollision(player, GetComponent<Collider2D>());
        anim = GetComponent<Animator>();
    }

    void Attack ()
    {

    }
}
