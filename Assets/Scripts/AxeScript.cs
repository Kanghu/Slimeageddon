using UnityEngine;
using System.Collections;

public class AxeScript : MonoBehaviour {

    public Collider2D myCollider;
    Transform myTransform;
    public Transform playerTransform;

    public bool faceRight;
    public bool returning = false;
    public float movement_speed = 8f;
    public int damage = 5;

	void Start () {
        myTransform = GetComponent<Transform>();
       // Physics2D.IgnoreCollision(player, GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update () {
        // Debug.Log(15f * Time.deltaTime);
        if (returning)
        {
            //myTransform.position = Vector3.Lerp(myTransform.position, playerTransform.position, 0.1f);
            myTransform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 14f * Time.deltaTime);
           // Debug.Log("should return at speed:" + 15f * Time.deltaTime);

        }

        else
        {

            if (faceRight)
            {
                myTransform.localPosition = new Vector3(myTransform.localPosition.x + movement_speed * Time.deltaTime, myTransform.localPosition.y, myTransform.localPosition.z);
            //    Debug.Log("! return RIGHT");
            }

            else
            {
                myTransform.localPosition = new Vector3(myTransform.localPosition.x - movement_speed * Time.deltaTime, myTransform.localPosition.y, myTransform.localPosition.z);
            //    Debug.Log("! return LEFT");
            }

           // Debug.Log("no return");
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ( collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Immovable Wall")
        {
            returning = true;
        }

        //Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Immovable Wall")
        {
            returning = true;
        }

        //Destroy(gameObject);
    }
}
