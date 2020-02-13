using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeColliderScript : MonoBehaviour {

    public float damage;
    public ClassWizard parentWiz;

	// Use this for initialization
	void Start () {
        damage = parentWiz.freezeDamage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player 1")
        {
            StartCoroutine(collision.gameObject.GetComponent<PlayerBehaviour>().takeFreeze(damage, 0.1f));
            //collision.gameObject.GetComponent<PlayerBehaviour>().freeze(5f);
        }
    }
}
