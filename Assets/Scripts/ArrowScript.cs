using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

    SpriteRenderer render;
    Rigidbody2D rbody;

	// Use this for initialization
	void Start () {
        render = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!render.isVisible)
        {
            
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "TRWall")
        {
            rbody.velocity = new Vector2(0, 0); // se opreste cand se loveste de ceva
            rbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // infigere in obiect

            //transform.SetParent(collision.transform);

            Invoke("DestroyObj", 3f); // autodistrugere dupa cateva secunde
        }
        
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}
