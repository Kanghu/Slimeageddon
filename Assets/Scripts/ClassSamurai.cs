using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ClassSamurai : NetworkBehaviour {


    PlayerBehaviour playerB;
    public GameObject weaponS;
    public GameObject weaponHB;
    CooldownBH CD;

    public float DashSpeedX;
    public float DashSpeedY;

    public float _ofset = 0.3f;
    float ofset = 0.3f;
    public GameObject swirling_shuriken;
    public bool dashing;
    public int dash_damage = 5;
    public float invisibleTime = 2.0f;

    public float dashMana = 5f;
    public float shurikenMana = 5f;
    public float invisibleMana = 5f;

    public bool invisible = true;
    public bool dashCol = false;

    public bool dashFinished = true;

    // Use this for initialization
    void Start () {

        CD = GetComponent<CooldownBH>();

        playerB = GetComponent<PlayerBehaviour>();
        playerB.attack_anim = 0.35f;
        weaponS = playerB.weapon_object;
        weaponHB = playerB.weapon_hitbox.gameObject;

    }

    void Invisible()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        playerB.immovable = false;

        weaponS.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        weaponHB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

        weaponS.SetActive(true);
        weaponHB.SetActive(true);
    }

    void FullInvisible()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);
        playerB.immovable = false;

        weaponS.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);
        weaponHB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);

        weaponS.SetActive(true);
        weaponHB.SetActive(true);
    }

    void Visible()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        weaponS.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        weaponHB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
    }

    void DashOff()
    {
        weaponS.SetActive(true);

        if (isServer)
        {
            RpcDash(false);
            RpcDashAttack(false);
        }

        else
        {
            dashing = false;
            playerB.immovable = false;
            Physics2D.IgnoreLayerCollision(8, 8, false);
            weaponHB.GetComponent<Animator>().SetBool("dashing", false);

            CmdDash(false);
            CmdDashAttack(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer || playerB.dead || !playerB.serverStarted)
            return;

        if (invisible && Time.time > CD.Start_Q + CD.Duration_Q)
        {
            invisible = false;

            if (isServer)
            {
                RpcVisible();
            }

            else
            {
                Visible();
                CmdVisible();
            }
        }

        if(dashing && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Samurai Dash"))
        {
            DashOff();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if ( (Time.time > CD.Start_M2 + CD.Cooldown_M2 || CD.Start_M2 == 0f) && (playerB.mana >= dashMana) )
            {
                CD.Start_M2 = Time.time;
                playerB.mana -= dashMana;

                GetComponent<Animator>().Play("Samurai Dash");
                weaponS.SetActive(false);

                if (isServer)
                {
                    RpcDash(true);
                    RpcDashAttack(true);
                }

                else
                {
                    dashing = true;
                    playerB.immovable = true;
                    Physics2D.IgnoreLayerCollision(8, 8, true);
                    weaponHB.GetComponent<Animator>().SetBool("dashing", true);

                    CmdDash(true);
                    CmdDashAttack(true);
                }

                if (playerB.faceRight)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(DashSpeedX, DashSpeedY);

                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-DashSpeedX, DashSpeedY);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if ( (Time.time > CD.Start_Q + CD.Cooldown_Q || CD.Start_Q == 0f) && (playerB.mana >= invisibleMana) )
            {
                CD.Start_Q = Time.time;
                playerB.mana -= invisibleMana;
                invisible = true;

                if (isServer)
                {
                    RpcInvisible();

                    Invoke("Invisible", 1.2f);
                }

                else
                {
                    playerB.immovable = true;

                    weaponS.SetActive(false);
                    weaponHB.SetActive(false);

                    GetComponent<Animator>().Play("Samurai Invisibility", 0, 0);

                    CmdInvisible();

                    Invoke("Invisible", 1.2f);
                }
            }
           // GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

            
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if ( (Time.time > CD.Start_E + CD.Cooldown_E || CD.Start_E == 0f) && (playerB.mana >= shurikenMana) )
            {
                CD.Start_E = Time.time;
                playerB.mana -= shurikenMana;

                if (playerB.faceRight)
                    ofset = _ofset;
                else
                    ofset = - _ofset;

                if (isServer)
                    RpcWhirle(ofset);

                else
                {
                    GameObject shuriken;
                    shuriken = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken.GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken.GetComponent<Collider2D>());
                    shuriken.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
                    shuriken.GetComponent<ShurikenScript>().shurikenNr = 1;
                    shuriken.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
                    shuriken.SetActive(true);
                    shuriken.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);


                    GameObject shuriken2;
                    shuriken2 = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken2.GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken2.GetComponent<Collider2D>());
                    shuriken2.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
                    shuriken2.GetComponent<ShurikenScript>().shurikenNr = 2;
                    shuriken2.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
                    shuriken2.SetActive(true);
                    shuriken2.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);

                    GameObject shuriken3;
                    shuriken3 = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken3.GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken3.GetComponent<Collider2D>());
                    shuriken3.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
                    shuriken3.GetComponent<ShurikenScript>().shurikenNr = 3;
                    shuriken3.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
                    shuriken3.SetActive(true);
                    shuriken3.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);


                    CmdWhirle(ofset);
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player 1")
        {
            if(dashing)
            {
                StartCoroutine(collision.gameObject.GetComponent<PlayerBehaviour>().takeDamage(dash_damage));
            }
        }

        else if (collision.gameObject.tag == "Axe")
        {
            StartCoroutine(GetComponent<PlayerBehaviour>().takeDamage(10));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Axe")
        {
            StartCoroutine(GetComponent<PlayerBehaviour>().takeDamage(10));
        }
    }


    [Command]
    public void CmdWhirle(float ofset)
    {
        GameObject shuriken;
        shuriken = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken.GetComponent<Collider2D>());
        shuriken.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
        shuriken.GetComponent<ShurikenScript>().shurikenNr = 1;
        shuriken.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
        shuriken.SetActive(true);
        shuriken.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);


        GameObject shuriken2;
        shuriken2 = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken2.GetComponent<Collider2D>());
        shuriken2.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
        shuriken2.GetComponent<ShurikenScript>().shurikenNr = 2;
        shuriken2.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
        shuriken2.SetActive(true);
        shuriken2.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);

        GameObject shuriken3;
        shuriken3 = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken3.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken3.GetComponent<Collider2D>());
        shuriken3.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
        shuriken3.GetComponent<ShurikenScript>().shurikenNr = 3;
        shuriken3.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
        shuriken3.SetActive(true);
        shuriken3.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);
    }


    [ClientRpc]
    public void RpcWhirle(float ofset)
    {
        GameObject shuriken;
        shuriken = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken.GetComponent<Collider2D>());
        shuriken.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
        shuriken.GetComponent<ShurikenScript>().shurikenNr = 1;
        shuriken.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
        shuriken.SetActive(true);
        shuriken.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);


        GameObject shuriken2;
        shuriken2 = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken2.GetComponent<Collider2D>());
        shuriken2.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
        shuriken2.GetComponent<ShurikenScript>().shurikenNr = 2;
        shuriken2.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
        shuriken2.SetActive(true);
        shuriken2.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);

        GameObject shuriken3;
        shuriken3 = Instantiate(swirling_shuriken, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shuriken3.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), shuriken3.GetComponent<Collider2D>());
        shuriken3.GetComponent<ShurikenScript>().faceRight = playerB.faceRight;
        shuriken3.GetComponent<ShurikenScript>().shurikenNr = 3;
        shuriken3.GetComponent<ShurikenScript>().playerTransform = GetComponent<Transform>();
        shuriken3.SetActive(true);
        shuriken3.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.05f, transform.position.z);
    }

    [Command]
    public void CmdInvisible()
    {
        playerB.immovable = true;

        weaponS.SetActive(false);
        weaponHB.SetActive(false);

        GetComponent<Animator>().Play("Samurai Invisibility", 0, 0);


        Invoke("FullInvisible", 1.1f);
    }

    [ClientRpc]
    public void RpcInvisible()
    {
        playerB.immovable = true;

        weaponS.SetActive(false);
        weaponHB.SetActive(false);

        GetComponent<Animator>().Play("Samurai Invisibility", 0, 0);


        Invoke("FullInvisible", 1.1f);
    }

    [ClientRpc]
    public void RpcVisible()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        if(weaponS)
            weaponS.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        if(weaponHB)
        weaponHB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
    }

    [Command]
    public void CmdVisible()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        if (weaponS)
            weaponS.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        if (weaponHB)
            weaponHB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
    }

    [Command]
    public void CmdDash(bool ok)
    {
        dashing = ok;
        playerB.immovable = ok;

        Physics2D.IgnoreLayerCollision(8, 8, ok);
    }

    [ClientRpc]
    public void RpcDash(bool ok)
    {
        dashing = ok;
        playerB.immovable = ok;

        Physics2D.IgnoreLayerCollision(8, 8, ok);
    }

    [Command]
    public void CmdDashAttack(bool ok)
    {
        weaponHB.GetComponent<Animator>().SetBool("dashing", ok);

    }

    [ClientRpc]
    public void RpcDashAttack(bool ok)
    {
        weaponHB.GetComponent<Animator>().SetBool("dashing", ok);
    }
}
