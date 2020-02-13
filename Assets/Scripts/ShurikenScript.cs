using UnityEngine;
using System.Collections;

public class ShurikenScript : MonoBehaviour
{

    Transform myTransform;
    public Transform playerTransform;
    public int shurikenNr;

    public bool faceRight;
    public float movement_speed = 8f;
    public float separation_speed = 8f;
    public int damage = 5;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        
    }

    
    void Update()
    {
        if(shurikenNr == 1)
             if (faceRight)
                      myTransform.localPosition = new Vector3(myTransform.localPosition.x + movement_speed * Time.deltaTime, myTransform.localPosition.y - separation_speed * Time.deltaTime, myTransform.localPosition.z);

              else
                      myTransform.localPosition = new Vector3(myTransform.localPosition.x - movement_speed * Time.deltaTime, myTransform.localPosition.y - separation_speed * Time.deltaTime, myTransform.localPosition.z);

        else if (shurikenNr == 2)
            if (faceRight)
                myTransform.localPosition = new Vector3(myTransform.localPosition.x + movement_speed * Time.deltaTime, myTransform.localPosition.y, myTransform.localPosition.z);

            else
                myTransform.localPosition = new Vector3(myTransform.localPosition.x - movement_speed * Time.deltaTime, myTransform.localPosition.y, myTransform.localPosition.z);

        else if (shurikenNr == 3)
            if (faceRight)
                myTransform.localPosition = new Vector3(myTransform.localPosition.x + movement_speed * Time.deltaTime, myTransform.localPosition.y + separation_speed * Time.deltaTime, myTransform.localPosition.z);

            else
                myTransform.localPosition = new Vector3(myTransform.localPosition.x - movement_speed * Time.deltaTime, myTransform.localPosition.y + separation_speed * Time.deltaTime, myTransform.localPosition.z);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "TRWall" && collision.gameObject.tag != "Potion" && collision.gameObject.tag != "Portal" && collision.gameObject.tag != "Shuriken")
            Destroy(gameObject);

        if (collision.gameObject.tag == "Player 1")
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().StartCoroutine(collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(damage));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "TRWall" && collision.gameObject.tag != "Potion" && collision.gameObject.tag != "Portal" && collision.gameObject.tag != "Shuriken")
            Destroy(gameObject);

        if (collision.gameObject.tag == "Player 1")
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().StartCoroutine(collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(damage));
        }
    }
}
