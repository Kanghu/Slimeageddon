using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {


    public GameObject Bullet;
    public float ShootForce = 5;
    private Transform _myTransform;
    private Vector2 _lookDirection;
    public float destroyDelay = 1f;
    public float fireRate = 1.5f;
    public GameObject POF;
    float timeToFire = 0;
   // float timeToFire2 = 0;
    public AudioClip shootSound;
    //AudioSource source;
    public float ShootVolume = 0.7f;
    

    private void Start()
    {
        //Bullet = GameObject.Find("PlayerBullet");
        //POF = GameObject.Find("PointOfFire");
        if (!Bullet)
        {
            Debug.LogError("Bullet is not assigned to the script!");
        }

        _myTransform = POF.transform;
        //source = GetComponent<AudioSource>();


        

    }

    private void FixedUpdate()
    {
       
        var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        
        
        var screenPos = Camera.main.WorldToScreenPoint(_myTransform.position);
        var screenPos2D = new Vector2(screenPos.x, screenPos.y);

        
        _lookDirection = mousePos - screenPos2D;

        // Normalize the look dir.
        _lookDirection.Normalize();
       // Debug.Log("L: " + _lookDirection);
      //  Debug.Log("P: " + screenPos2D);
       // Debug.Log("E: " + mousePos);

    }

    void Update()
    {
        
        if (fireRate == 0)
        {
            if (Input.GetButton("Fire2"))

                Shoot();
        }
        else
        {
        
            if (Input.GetButton("Fire2") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
        
    }




    void Shoot()
    {
      //  if (source.isPlaying == false)
      //      source.PlayOneShot(shootSound, ShootVolume);

        var bullet = Instantiate(Bullet, _myTransform.position,  Quaternion.Euler(0, 0, Mathf.Atan2( _lookDirection.y  ,  _lookDirection.x) *  Mathf.Rad2Deg  - 45  )) as GameObject;

        

        if (bullet)
        {
            // Ignore collision
           // Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // Get the Rigid.2D reference
            var rigid = bullet.GetComponent<Rigidbody2D>();

            // Add forect to the rigidbody (As impulse).
            rigid.AddForce(_lookDirection * ShootForce, ForceMode2D.Impulse);

            // Destroy bullet after *destroyDelay* sec.
            //Destroy(bullet, destroyDelay);
        }
    }

   
}
