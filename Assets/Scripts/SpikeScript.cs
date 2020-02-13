using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {


    public float Damage;
    float lastDmg = 0f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("sup");
        if (collision.gameObject.tag == "Player 1" && Time.time > lastDmg + 0.2f)
        {
            Debug.Log("bnu");
            lastDmg = Time.time;
            StartCoroutine(collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(Damage));
            
        }
    }
}
