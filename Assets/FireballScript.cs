using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {

    public float movement_speed = 5f;
    public float damage = 10f;
    public bool faceRight;
    Transform myTransform;

    public bool finished = false;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (finished)
            DestroyObject(gameObject);

        if (faceRight)
        {
            myTransform.localPosition = new Vector3(myTransform.localPosition.x + movement_speed * Time.deltaTime, myTransform.localPosition.y, myTransform.localPosition.z);
        }

        else
        {
            myTransform.localPosition = new Vector3(myTransform.localPosition.x - movement_speed * Time.deltaTime, myTransform.localPosition.y, myTransform.localPosition.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player 1")
        {
            StartCoroutine(collision.GetComponent<PlayerBehaviour>().takeDamage(damage));
        }

    }
}
