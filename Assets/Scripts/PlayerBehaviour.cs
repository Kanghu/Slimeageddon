using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerBehaviour : NetworkBehaviour {

    public Sprite[] skillIcons;

    public Color32 colorParticles;
    public Animator partAnim;

    GameManager GM;
    public bool serverStarted = false;

    Transform testWarp;
    Animator anim;
    BoxCollider2D box;
    Transform PlayerRect;
    ParticleSystem particle;
    SpriteRenderer sprite;

    public Sprite spriteOriginal;

    Vector2 dirVec; // vectorul pentru Input
    Vector2 facingVec; // vectorul folosit de sageti

    float movement_vector;
    //Rigidbody2D rigidBody;

    public Rigidbody2D arrow;
    public WeaponScript weapon;


    public Animator weapon_hitbox;
    public GameObject weapon_object;

    [SyncVar]
    public string PlayerName;

    public int playerNr;
    public float movement_speed = 0.02f;
    public float attack_speed = 1f;
    public int canJump = 0;
    public float currJump = 0;
    public bool immovable = false;

    [SyncVar]
    public bool dead = false;

    [SyncVar]
    public float mana;

    [SyncVar]
    public float health;

    [SyncVar]
    public bool frozen;

    public float fullMana = 100f;
    public float fullHealth = 250f;
    public float basicDamage = 15f;

    public float VelocityPerJump;
    public bool recharging;
    public bool faceRight = true;
    public Transform myTransform;
    public Vector3 startPoint;

    public bool connected = false;
    public float attack_anim;

    public GameObject prefabTest;
    public Button respButton;

    [SyncVar]
    public int freezeGrade;

    public float startFreeze;

    public int myScore;
    public float defMovSpeed;

    public GameObject PopUp;

    // Use this for initialization
    void Start () {

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        partAnim.GetComponent<SpriteRenderer>().color = colorParticles;

        GameObject portal = GameObject.Find("Map/Portal");

        if (portal.GetComponent<Collider2D>().enabled == false)
        {
            portal.GetComponent<PortalScript>().goActive = true;
        }

        // GM.InitPlayer(this);

        gameObject.tag = "Player 1" ;
        myScore = 0;

        if (isServer)
            immovable = true;
        else
            immovable = false;

        mana = fullMana;
        health = fullHealth;

        weapon = GetComponentInChildren<WeaponScript>();
        anim = GetComponent<Animator>();
        PlayerRect = GetComponent<Transform>();
        particle = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        myTransform = GetComponent<Transform>();
       // rigidBody = GetComponent<Rigidbody2D>();

        set_AttSpeed(attack_speed);



        // weapon_hitbox.gameObject.SetActive(false);
        //    particle.Pause();

        if (!isLocalPlayer)
            return;

        GameObject.Find("Player UI's/Player UI/Cooldowns/CD M2/Life BG M2").GetComponent<Image>().sprite = skillIcons[0];
        GameObject.Find("Player UI's/Player UI/Cooldowns/CD Q/Life BG Q").GetComponent<Image>().sprite = skillIcons[1];
        GameObject.Find("Player UI's/Player UI/Cooldowns/CD E/Life BG E").GetComponent<Image>().sprite = skillIcons[2];
    }

    private void FixedUpdate()
    {

        if (!isLocalPlayer || dead) // misca doar caracter-ul propriului client
            return;

        if (!immovable)
        {
            dirVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float movement_vector = dirVec.x;

            if (movement_vector < 0)
                movement_vector = -movement_vector;
            anim.SetFloat("input_x", movement_vector);
        }

        if ( !((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))))
        {
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !immovable)
            {
                //PlayerRect.localPosition = new Vector3(PlayerRect.localPosition.x + movement_speed * Time.deltaTime, PlayerRect.localPosition.y, PlayerRect.localPosition.z);
                PlayerRect.Translate(new Vector3(movement_speed * Time.deltaTime, 0, 0));

                if (!faceRight)
                {
                    if (isServer)
                    {
                        RpcTurn();
                    }

                    else
                    {
                        CmdTurn();

                        myTransform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        faceRight = true;
                        facingVec.x = 1;
                    }
                }
            }

            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !immovable)
            {
                //PlayerRect.localPosition = new Vector3(PlayerRect.localPosition.x - movement_speed * Time.deltaTime, PlayerRect.localPosition.y, PlayerRect.localPosition.z);
                PlayerRect.Translate(new Vector3(-movement_speed * Time.deltaTime, 0, 0));

                if (faceRight)
                {
                    if (isServer)
                    {
                        RpcTurn();
                    }

                    else
                    {
                        CmdTurn();

                        myTransform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        faceRight = false;
                        facingVec.x = 1;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            connected = true;
            serverStarted = true;
            immovable = false;
        }

        if (!isLocalPlayer || dead)
            return;

       /* if (health < 0 && !dead)
        {
            dead = true;
            Die();
        }*/


        if (!connected && NetworkServer.connections.Count > 1 && isServer)
        {
            connected = true;
            serverStarted = true;
            immovable = false;
        }

        else if(!isServer)
        {
            serverStarted = true;
        }

        if (!serverStarted)
            return;

        if (recharging && mana < fullMana)
        {
            mana += 0.5f;
        }

        if(frozen && Time.time > startFreeze + 3f)
        {
            if (isServer)
                RpcDefrost();
            else
            {
                anim.SetBool("frozen", false);

                movement_speed = defMovSpeed;
                freezeGrade = 0;
                frozen = false;
                immovable = false;
                sprite.color = new Color32(255, 255, 255, 255);
                CmdDefrost();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !immovable)
        {
            if ( (Time.time - currJump > 0.25f || canJump == 1) && canJump < 2)
            {
                GetComponent<Animator>().Play("Jumping", 0 , 0);

                if (canJump == 0)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, VelocityPerJump);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, VelocityPerJump / 1.35f);
                }

                canJump++;
                currJump = Time.time;
            }
        }

        if (Input.GetButton("Fire1") && !immovable)
        {
            if (weapon.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (isServer)
                    RpcAttack();

                else
                {
                    CmdAttack();

              //      weapon_hitbox.gameObject.SetActive(true);
                    weapon.anim.Play("Hit", 0, 0);
                    weapon_hitbox.Play("Attack", 0, 0);
           //         Invoke("DisableWeapon", attack_anim);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.S) && !immovable)
        {
            anim.Play("Recharge", 0, 0);
            anim.SetBool("recharging", true);

            recharging = true;
            immovable = true;
        }

        if(Input.GetKeyUp(KeyCode.S) && recharging)
        {
            anim.SetBool("recharging", false);

            recharging = false;
            immovable = false;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {

            /*
            StartCoroutine(takeDamage(10));
            immovable = !immovable;
            */
           // Die();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "TRWall")
            canJump = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = 0;
        }

        else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "TRWall")
            canJump = 0;

        else if (collision.gameObject.tag == "Potion")
        {
            if (collision.gameObject.GetComponent<PotionScript>().type == 1)
            {
                if (!faceRight)
                {
                    PopUp.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    PopUp.GetComponent<TextMesh>().anchor = TextAnchor.UpperRight;
                }
                else if (faceRight)
                {
                    PopUp.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    PopUp.GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                }

                PopUp.GetComponent<TextMesh>().text = "Health +50";
                PopUp.GetComponent<Animator>().Play("Health", 0, 0);

                health += 50;
            }

            else if(collision.gameObject.GetComponent<PotionScript>().type == 2)
            {
                if (!faceRight)
                {
                    PopUp.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    PopUp.GetComponent<TextMesh>().anchor = TextAnchor.UpperRight;
                }
                else if (faceRight)
                {
                    PopUp.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    PopUp.GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                }

                PopUp.GetComponent<TextMesh>().text = "Damage +10";
                PopUp.GetComponent<TextMesh>().color = new Color32(0, 0, 255, 255);
                PopUp.GetComponent<Animator>().Play("Health", 0, 0);

                basicDamage += 10f;
            }
        }
    }

    public IEnumerator takeDamage(float damage)
    {
        if (dead)
            yield return null;

        health -= damage;
        sprite.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color32(255, 255, 255, 255);

        partAnim.Play("Slime Take Dmg " + ( (int) UnityEngine.Random.Range(1, 4)).ToString());

        if (health <= 0 && !dead)
        {
            dead = true;
            KillAll();
            Die();
        }

    }

    public IEnumerator takeFreeze(float damage, float slow)
    {
        if (dead)
            yield return null;

        health -= damage;

        sprite.color = new Color32(0, 0, 255, 255);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color32(255, 255, 255, 255);

        if (freezeGrade < 255)
        {
            freezeGrade += 25;
        }

        else
        {
            anim.Play("Frozen", 0, 0);
            anim.SetBool("frozen", true);
            immovable = true;
        }

        startFreeze = Time.time;
        frozen = true;
        movement_speed -= slow;

        if (health <= 0 && !dead)
        {
            KillAll();
            Die();
        }

    }

    void DisableWeapon()
    {
        weapon_hitbox.gameObject.SetActive(false);
    }


    void set_AttSpeed(float Speed)
    {
        weapon.GetComponent<Animator>().speed = attack_speed;
        weapon_hitbox.speed = attack_speed;
    }



    public void Stun(float period)
    {
        if (isServer)
            RpcStun(true);

        else
        {
            CmdStun(true);
            immovable = true;
        }

        anim.Play("Daze");
        anim.SetBool("dazing", true);

        Invoke("StopStun", period);
    }


    void StopStun()
    {
        if (isServer)
            RpcStun(false);

        else
        {
            CmdStun(false);
            immovable = false;
        }

        anim.SetBool("dazing", false);
    }


    public void Die()
    {
       if (!isLocalPlayer)
            return;

        immovable = true;
        dead = true;
        anim.SetBool("dead", true);
        anim.Play("Death", 0, 0);

        if (isServer)
        {
            RpcSetWeaponObj(false);
        }

        else
        {
            weapon_object.GetComponent<SpriteRenderer>().enabled = false;
            CmdSetWeaponObj(false);
        }

        DisableWeapon();

        Invoke("RespawnCanvasL", 1.5f);
    }

    public void Win()
    {
        if (!isLocalPlayer)
            return;

        if (isServer)
        {
            RpcAddScore();
        }

        else
        {
            myScore += 1;
            CmdAddScore();
        }

        immovable = true;
        dead = true;

        Invoke("RespawnCanvasW", 1.5f);
    }

    public void RespawnCanvasW()
    {
        GameObject.Find("Respawn Canvas").GetComponent<Canvas>().sortingOrder = 500;
        GameObject.Find("Respawn Canvas").GetComponent<RespawnCanvasScript>().result = true;
        GameObject.Find("Respawn").GetComponent<Button>().onClick.AddListener(delegate { RespawnV(); });
    }

    public void RespawnCanvasL()
    {
        GameObject.Find("Respawn Canvas").GetComponent<Canvas>().sortingOrder = 500;
        GameObject.Find("Respawn Canvas").GetComponent<RespawnCanvasScript>().result = false;
        GameObject.Find("Respawn").GetComponent<Button>().onClick.AddListener(delegate { RespawnV(); });
    }

    public void RespawnV()
    {
        int pick = GameObject.Find("Respawn Canvas").GetComponent<RespawnCanvasScript>().picked;
        GameObject.Find("Respawn Canvas").GetComponent<Canvas>().sortingOrder = -100;
        CmdRespawn(pick);
    }

    public override void OnStartLocalPlayer()
    {
        //GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        //GM.InitPlayer(this);

        InitPlayer();


        //GameObject.Find("Menus/Character Selection/InputField").GetComponent<Image>().raycastTarget = false;

        Camera.main.GetComponent<CameraFollow>().setTarget(transform); // propria camera urmareste propriul caracter
        
        GameObject.Find("Health").GetComponent<HPBar>().setTarget(this);
        GameObject.Find("Mana").GetComponent<ManaBar>().setTarget(this);
        GameObject.Find("Cooldowns").GetComponent<CDBar>().setTarget(GetComponent<CooldownBH>());
        GameObject.Find("My Score").GetComponent<ScoreScript>().setTarget(this);

        startPoint = transform.position;
        CmdSpawnPoint();
    }


    void InitPlayer()
    {
        GameObject canvasChSel = GameObject.Find("Menus/Character Selection");

        if (canvasChSel && canvasChSel.active == true)
        {
            string name = GameObject.Find("Menus/Character Selection/InputField PlayerName").GetComponent<InputField>().text;

            if (name == string.Empty)
                name = "Player";

            PlayerName = name;
            this.name = PlayerName;

            GameObject.Find("Player UI's/Player UI/My Player Name/Text").GetComponent<Text>().text = name;
            CmdName(name);
        }

        //Disable Character Select
        if (canvasChSel && canvasChSel.active == true)
        {
            canvasChSel.SetActive(false);
        }
        
    }

    [Command]
    public void CmdName(string selectedName)
    {
        PlayerName = selectedName;
        this.name = PlayerName;
    }



    [Command]
    public void CmdTurn()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        faceRight = !faceRight;
        facingVec.x = 1;
    }

    [ClientRpc]
    public void RpcTurn()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        faceRight = !faceRight;
        facingVec.x = 1;
    }

    [Command]
    public void CmdAttack()
    {
      //  weapon_hitbox.gameObject.SetActive(true);
        weapon.anim.Play("Hit", 0, 0);
        weapon_hitbox.Play("Attack", 0, 0);
      //  Invoke("DisableWeapon", attack_anim);
    }

    [ClientRpc]
    public void RpcAttack()
    {
       // weapon_hitbox.gameObject.SetActive(true);
        weapon.anim.Play("Hit", 0, 0);
        weapon_hitbox.Play("Attack", 0, 0);
        //Invoke("DisableWeapon", attack_anim);
    }

    [Command]
    public void CmdStun(bool how)
    {
        immovable = how;
    }

    [ClientRpc]
    public void RpcStun(bool how)
    {
        immovable = how;
    }

    [Command]
    public void CmdActivateWeapon()
    {
        weapon_object.GetComponent<SpriteRenderer>().enabled = true;
        weapon_hitbox.gameObject.SetActive(true);

        health = fullHealth;
        mana = fullMana;
        dead = false;
        immovable = false;
    }

    [ClientRpc]
    public void RpcActivateWeapon()
    {
        weapon_object.GetComponent<SpriteRenderer>().enabled = true;
        weapon_hitbox.gameObject.SetActive(true);
    }

    [Command]
    public void CmdSetWeaponObj(bool ok)
    {
        weapon_object.GetComponent<SpriteRenderer>().enabled = ok;
    }

    [ClientRpc]
    public void RpcSetWeaponObj(bool ok)
    {
        weapon_object.GetComponent<SpriteRenderer>().enabled = ok;
    }

    [Command]
    public void CmdRespawn(int pickedClass)
    {
        GameObject player;
        short conID = (short)(playerControllerId + 1);

        player = (GameObject)Instantiate(GameObject.Find("Network").GetComponent<NetworkScript>().spawnPrefabs[pickedClass], startPoint, transform.rotation);

        if (connected)
        {
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[0], player, conID);
        }

        else
        {
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[1], player, conID);
        }

        NetworkServer.Destroy(gameObject);
    }

    [Command]
    public void CmdSpawnPoint()
    {
        startPoint = transform.position;
    }

    [Command]
    public void CmdUpdateScore()
    {
        PlayerBehaviour[] targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

        for (int i = 0; i < targetsAvailable.Length; i++)
        {
            if (targetsAvailable[i] != this)
                targetsAvailable[i].myScore += 1;
        }
    }

    [ClientRpc]
    public void RpcUpdateScore()
    {
        PlayerBehaviour[] targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

        for (int i = 0; i < targetsAvailable.Length; i++)
        {
            if (targetsAvailable[i] != this)
                targetsAvailable[i].myScore += 1;
        }
    }

    [Command]
    public void CmdAddScore()
    {
        myScore += 1;
    }

    [ClientRpc]
    public void RpcAddScore()
    {
        myScore += 1;
    }

    [Command]
    public void CmdDefrost()
    {
        anim.SetBool("frozen", false);

        movement_speed = defMovSpeed;
        freezeGrade = 0;
        frozen = false;
        immovable = false;
        sprite.color = new Color32(255, 255, 255, 255);
    }

    [ClientRpc]
    public void RpcDefrost()
    {
        anim.SetBool("frozen", false);

        movement_speed = defMovSpeed;
        freezeGrade = 0;
        frozen = false;
        immovable = false;
        sprite.color = new Color32(255, 255, 255, 255);
    }

    void UpdateScore()
    {
        PlayerBehaviour[] targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

        for (int i = 0; i < targetsAvailable.Length; i++)
        {
            if (targetsAvailable[i] != this)
                targetsAvailable[i].myScore += 1;
        }
    }

    void ScoreUpdate()
    {
        if(isServer)
        {
            Debug.Log("Server updatin");
            RpcUpdateScore();
        }

        else
        {
            Debug.Log("client updatin");
            CmdUpdateScore();
            UpdateScore();
        }
    }

    void KillAll()
    {
        PlayerBehaviour[] targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

        for (int i = 0; i < targetsAvailable.Length; i++)
        {
            if (targetsAvailable[i] != this)
            {
                targetsAvailable[i].Win();
            }
        }
    }
}
