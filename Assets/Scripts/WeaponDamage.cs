using UnityEngine;
using System.Collections;

public class WeaponDamage : MonoBehaviour {

    public float damage;

    public PlayerBehaviour myPlayer;

    void Start()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player 1" && !gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") )
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(myPlayer.basicDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player 1")
        {
            StartCoroutine( collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(myPlayer.basicDamage) );

        }
    }

}
