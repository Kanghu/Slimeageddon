using UnityEngine;
using System.Collections;

public class PlatformTriggerScript : MonoBehaviour {

    public Collider2D myCollider;

	// Use this for initialization
	void Start () {

        myCollider = transform.parent.GetComponent<Collider2D>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player 1"))
        {
           // collider.isTrigger = true;
            Physics2D.IgnoreCollision(myCollider, collision, true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(myCollider, collision, false);
        //collider.isTrigger = false;
    }
}
