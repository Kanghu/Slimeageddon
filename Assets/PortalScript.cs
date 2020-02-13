using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

    public bool goActive = false;
    public bool goInactive = false;
    public Transform warpPoint;

	// Use this for initialization
	void Start () {

        // Invoke("Spawn");
        

    }
	
	// Update is called once per frame
	void Update () {
	
        if(goActive)
        {
            goActive = false;
            GetComponent<Animator>().Play("Spawn");
        }

        if (goInactive)
        {
            Debug.Log("despawn");

            goInactive = false;
            GetComponent<Animator>().Play("DeSpawn");
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Shuriken") || collision.gameObject.CompareTag("Axe") || collision.gameObject.CompareTag("Player 1"))
        {
            collision.transform.position = warpPoint.transform.position;

           

            goInactive = true;
            Invoke( "Spawn", Random.Range(15, 25) );

        }
        
    }


    public void Spawn()
    {
        goActive = true;

    }

}
