using UnityEngine;
using System.Collections;

public class StunColliderScript : MonoBehaviour {

    public CooldownBH CD;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player 1")
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().Stun(CD.Duration_Q);
        }
    }
}
