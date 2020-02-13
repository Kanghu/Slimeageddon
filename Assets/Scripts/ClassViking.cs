using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ClassViking : NetworkBehaviour {

    Animator anim;
    Transform PlayerRect;
    CooldownBH CD;

    public bool hasAxe = true;
    public GameObject swirling_axe;
    public Animator weapon_hitbox;
    public GameObject weapon_object;
    public GameObject stun_collider;
    
    PlayerBehaviour playerScript;

    public float stunMana = 5f;
    public float tpMana = 5f;
    public float whirleMana = 5f;



    void Start () {

        stun_collider.SetActive(false);

        CD = GetComponent<CooldownBH>();
        playerScript = GetComponent<PlayerBehaviour>();
        anim = GetComponent<Animator>();
        PlayerRect = GetComponent<Transform>();
        playerScript.attack_anim = 0.5f;


        swirling_axe = Instantiate(swirling_axe, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;

        swirling_axe.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            //anim.Play("Stomp", 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if( (Time.time > CD.Start_Q + CD.Cooldown_Q || CD.Start_Q == 0f) && (playerScript.mana > stunMana) )
            {
                CD.Start_Q = Time.time;
                playerScript.mana -= stunMana;

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
                {
                    if (isServer)
                        RpcStunCol(true);

                    else
                    {
                        CmdStunCol(true);
                        stun_collider.SetActive(true);
                    }

                    anim.Play("Stun", 0, 0);

                    Invoke("DisableStun", 0.416f);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if ((Time.time > CD.Start_E + CD.Cooldown_E || CD.Start_E == 0f) && (playerScript.mana > tpMana) && !swirling_axe.GetComponent<AxeScript>().returning)
            {
                CD.Start_E = Time.time;
                playerScript.mana -= tpMana;
                anim.Play("Teleport", 0, 0);

                /*swirling_axe.GetComponent<AxeScript>().faceRight = !swirling_axe.GetComponent<AxeScript>().faceRight;
                swirling_axe.GetComponent<AxeScript>().returning = true;
                arm_up();*/

                //Physics2D.IgnoreLayerCollision(8, 8, true);
                //Invoke("Teleport", 0.5f);
                //Teleport();

            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
                float ofset;

                if (playerScript.faceRight)
                    ofset = 0.5f;
                else
                    ofset = -0.5f;



                if (hasAxe && playerScript.weapon.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {

                    if ( (Time.time > CD.Start_M2 + CD.Cooldown_M2 || CD.Start_M2 == 0f) && (playerScript.mana > whirleMana) )
                    {
                        CD.Start_M2 = Time.time;
                        playerScript.mana -= whirleMana;

                            if (isServer)
                            {
                                RpcWhirle(ofset);
                            }

                            else
                            {
                                weapon_object.SetActive(false);
                                weapon_hitbox.gameObject.SetActive(false);

                                hasAxe = false;

                                swirling_axe.SetActive(true);
                                swirling_axe.GetComponent<AxeScript>().returning = false;
                                swirling_axe.GetComponent<AxeScript>().faceRight = playerScript.faceRight;
                                swirling_axe.GetComponent<AxeScript>().playerTransform = GetComponent<Transform>();
                                swirling_axe.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.15f, transform.position.z);

                                CmdWhirle(ofset);
                            }

                        swirling_axe.GetComponentInChildren<Camera>().enabled = true;
                   }
                }

                else
                {
                    if (!swirling_axe.GetComponent<AxeScript>().returning)
                    {
                        if (isServer)
                        {
                            RpcCome();
                        }   

                        else
                        {
                            swirling_axe.GetComponent<AxeScript>().faceRight = !swirling_axe.GetComponent<AxeScript>().faceRight;
                            swirling_axe.GetComponent<AxeScript>().returning = true;

                            CmdCome();
                        }
                    }
                }
            
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == swirling_axe)
        {
            Debug.Log("trigger");
            swirling_axe.SetActive(false);
            arm_up();
        }

        else if (collision.gameObject.tag == "Axe" && collision.gameObject != swirling_axe)
        {
            StartCoroutine(GetComponent<PlayerBehaviour>().takeDamage(10));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == swirling_axe)
        {
            swirling_axe.SetActive(false);
            arm_up();

        }

        else if (collision.gameObject.tag == "Axe" && collision.gameObject != swirling_axe)
        {
            StartCoroutine(GetComponent<PlayerBehaviour>().takeDamage(10));
        }
    }


    void arm_up()
    {
        weapon_object.SetActive(true);
        weapon_hitbox.gameObject.SetActive(true);
        hasAxe = true;
    }

    void DisableStun()
    {
        if (isServer)
            RpcStunCol(false);

        else
        {
            CmdStunCol(false);
            stun_collider.SetActive(false);
        }
    }

    void Teleport()
    {
        anim.Play("Teleport II");
        PlayerRect.position = swirling_axe.transform.position;
        //Invoke("TpCollision", 1f);
    }

    void TpCollision()
    {
        Physics2D.IgnoreLayerCollision(8, 8, false);
    }

    [Command]
    public void CmdWhirle(float ofset)
    {
        weapon_object.SetActive(false);
        weapon_hitbox.gameObject.SetActive(false);

        swirling_axe.SetActive(true);
        swirling_axe.GetComponent<AxeScript>().returning = false;
        swirling_axe.GetComponent<AxeScript>().faceRight = playerScript.faceRight;
        swirling_axe.GetComponent<AxeScript>().playerTransform = GetComponent<Transform>();
        swirling_axe.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.15f, transform.position.z);
    }

    [Command]
    public void CmdCome()
    {
        swirling_axe.GetComponent<AxeScript>().faceRight = !swirling_axe.GetComponent<AxeScript>().faceRight;
        swirling_axe.GetComponent<AxeScript>().returning = true;
    }
    
    [ClientRpc]
    public void RpcWhirle(float ofset)
    {
        weapon_object.SetActive(false);
        weapon_hitbox.gameObject.SetActive(false);

        swirling_axe.SetActive(true);
        swirling_axe.GetComponent<AxeScript>().returning = false;
        swirling_axe.GetComponent<AxeScript>().faceRight = playerScript.faceRight;
        swirling_axe.GetComponent<AxeScript>().playerTransform = GetComponent<Transform>();
        swirling_axe.transform.localPosition = new Vector3(transform.position.x + ofset, transform.position.y + 0.15f, transform.position.z);
    }

    [ClientRpc]
    public void RpcCome()
    {
        swirling_axe.GetComponent<AxeScript>().faceRight = !swirling_axe.GetComponent<AxeScript>().faceRight;
        swirling_axe.GetComponent<AxeScript>().returning = true;
    }

    [Command]
    public void CmdStunCol(bool ok)
    {
        stun_collider.SetActive(ok);
    }

    [ClientRpc]
    public void RpcStunCol(bool ok)
    {
        stun_collider.SetActive(ok);
    }

    public void CallAxe()
    {
        if (isServer)
            RpcCome();

        else
        {
            swirling_axe.GetComponent<AxeScript>().faceRight = !swirling_axe.GetComponent<AxeScript>().faceRight;
            swirling_axe.GetComponent<AxeScript>().returning = true;

            CmdCome();
        }
    }
}
