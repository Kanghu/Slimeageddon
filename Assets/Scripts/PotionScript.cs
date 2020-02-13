using UnityEngine;
using System.Collections;

public class PotionScript : MonoBehaviour
{

    public int type = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "Player 1")
        {
            Destroy(gameObject); // distruge potiunea de pe jos
        }
    }
}
