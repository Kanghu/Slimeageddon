using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ClassWizard : NetworkBehaviour
{

    Animator weaponAnim;
    Animator anim;
    Transform PlayerRect;
    CooldownBH CD;

    public bool hasAxe = true;
    
    public Animator weapon_hitbox;
    public GameObject weapon_object;

    public GameObject iceLance;
    public GameObject fireBall;
    public float fireballOffset = 0.5f;

    PlayerBehaviour playerB;

    public float barrierMana = 5f;
    public float lanceMana = 5f;
    public float fballMana = 5f;

    public float freezeMana = 0.5f;
    public float fbDamage = 25f;
    public float freezeDamage = 3f;

    GameObject iceLanceGO;
    public bool castingFreeze = false;
    public Collider2D freezeCollider;

    void Start()
    {

        CD = GetComponent<CooldownBH>();
        playerB = GetComponent<PlayerBehaviour>();
        anim = GetComponent<Animator>();
        PlayerRect = GetComponent<Transform>();
        weaponAnim = weapon_object.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer || playerB.dead || !playerB.serverStarted)
            return;

        if (castingFreeze)
        {
            playerB.mana -= freezeMana;

            if (playerB.mana <= 0)
            { 
                if (isServer)
                {
                    RpcLanceOff();
                }

                else
                {
                    //playerB.immovable = false;
                    //  weapon_object.SetActive(true);
                    castingFreeze = false;
                    weaponAnim.Play("Idle", 0, 0);
                    weaponAnim.SetBool("freezing", false);

                    CmdLanceOff();
                }
            } 
        }

        else
        {
            if (freezeCollider.enabled)
                freezeCollider.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.Play("Barrier", 0, 0);
        }

        if (Input.GetButtonDown("Fire2"))
        {
           

            if ((Time.time > CD.Start_M2 + CD.Cooldown_M2 || CD.Start_M2 == 0f) && (playerB.mana >= lanceMana))
            {
                CD.Start_M2 = Time.time;
                playerB.mana -= lanceMana;

                if (isServer)
                {
                    RpcLance();
                }

                else
                {
                    //playerB.immovable = true;
                    // weapon_object.SetActive(false);
                    castingFreeze = true;
                    weaponAnim.Play("Freeze", 0, 0);
                    weaponAnim.SetBool("freezing", true);

                    CmdLance();
                }
            }
        }

        if(Input.GetButtonUp("Fire2"))
        {
            if (isServer)
            {
                RpcLanceOff();
            }

            else
            {
                //playerB.immovable = false;
                //  weapon_object.SetActive(true);
                castingFreeze = false;
                weaponAnim.Play("Idle", 0, 0);
                weaponAnim.SetBool("freezing", false);

                CmdLanceOff();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            float offset;

            if (playerB.faceRight)
                offset = fireballOffset;

            else
                offset = -fireballOffset;

            if ((Time.time > CD.Start_E + CD.Cooldown_E || CD.Start_E == 0f) && (playerB.mana >= fballMana))
            {
                CD.Start_E = Time.time;
                playerB.mana -= fballMana;

                if (isServer)
                {
                    RpcFireball(offset);
                }

                else
                {
                    weaponAnim.Play("Hit", 0, 0);
                    var fireball = Instantiate(fireBall, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    fireball.transform.localPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
                    fireball.GetComponent<FireballScript>().damage = fbDamage;
                    fireball.GetComponent<FireballScript>().faceRight = playerB.faceRight;

                    if (!playerB.faceRight)
                    {
                        fireball.transform.localScale = new Vector3(-fireball.transform.localScale.x, fireball.transform.localScale.y, fireball.transform.localScale.z);
                    }

                    CmdFireball(offset);
                }
            }
        }

    }

    /*[ClientRpc]
    public void RpcLance(float offset)
    {
        playerB.immovable = true;
        iceLanceGO = Instantiate(iceLance, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        iceLanceGO.transform.localPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);

        if(!playerB.faceRight)
            iceLanceGO.transform.localScale = new Vector3(-iceLanceGO.transform.localScale.x, iceLanceGO.transform.localScale.y, iceLanceGO.transform.localScale.z);


    }


    [Command] 
    public void CmdLance(float offset)
    {
        playerB.immovable = true;
        iceLanceGO = Instantiate(iceLance, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        iceLanceGO.transform.localPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);

        if (!playerB.faceRight)
            iceLanceGO.transform.localScale = new Vector3(-iceLanceGO.transform.localScale.x, iceLanceGO.transform.localScale.y, iceLanceGO.transform.localScale.z);

     
    }*/

    [ClientRpc]
    public void RpcLance()
    {
        //playerB.immovable = true;
        //weapon_object.SetActive(false);
        castingFreeze = true;
        weaponAnim.Play("Freeze", 0, 0);
        weaponAnim.SetBool("freezing", true);
    }


    [Command]
    public void CmdLance()
    {
        //playerB.immovable = true;
        //weapon_object.SetActive(false);
        castingFreeze = true;
        weaponAnim.Play("Freeze", 0, 0);
        weaponAnim.SetBool("freezing", true);
    }

    [ClientRpc]
    public void RpcLanceOff()
    {
        //playerB.immovable = false;
        //weapon_object.SetActive(true);
        castingFreeze = false;
        weaponAnim.Play("Idle", 0, 0);
        weaponAnim.SetBool("freezing", false);
    }


    [Command]
    public void CmdLanceOff()
    {
        //playerB.immovable = false;
        //weapon_object.SetActive(true);
        castingFreeze = false;
        weaponAnim.Play("Idle", 0, 0);
        weaponAnim.SetBool("freezing", false);
    }

    [Command]
    public void CmdFireball(float offset)
    {
        var fireball = Instantiate(fireBall, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        fireball.transform.localPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        fireball.GetComponent<FireballScript>().faceRight = playerB.faceRight;
        fireball.GetComponent<FireballScript>().damage = fbDamage;
        weaponAnim.Play("Hit", 0, 0);

        if (!playerB.faceRight)
        {
            fireball.transform.localScale = new Vector3( - fireball.transform.localScale.x, fireball.transform.localScale.y, fireball.transform.localScale.z);
        }
    }

    [ClientRpc]
    public void RpcFireball(float offset)
    {
        var fireball = Instantiate(fireBall, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        fireball.transform.localPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        fireball.GetComponent<FireballScript>().faceRight = playerB.faceRight;
        fireball.GetComponent<FireballScript>().damage = fbDamage;
        weaponAnim.Play("Hit", 0, 0);

        if (!playerB.faceRight)
        {
            fireball.transform.localScale = new Vector3(-fireball.transform.localScale.x, fireball.transform.localScale.y, fireball.transform.localScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
    }


  
}
