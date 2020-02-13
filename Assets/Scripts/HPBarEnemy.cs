using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBarEnemy : MonoBehaviour
{
    public Text count;
    public Image HPSlider;

    public bool targetFound = false;
    public PlayerBehaviour target;
    public PlayerBehaviour target_Main;
    public PlayerBehaviour[] targetsAvailable;

    public Text EnemyName;

    private Image FillVal;
    Canvas cg;

    public float HPFull;
    public float HPCurrent;


   

    private void Start()
    {
       cg = GetComponentInParent<Canvas>();
       cg.enabled = false;

        Invoke("findTarget", 0.5f);
    }

    private void findTarget()
    {
        
        if(target_Main == null)
        {
            target_Main = GameObject.Find("Health").GetComponent<HPBar>().target;
        }

        if (target == null)
        {
            targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

            for (int i = 0; i < targetsAvailable.Length; i++)
                if (targetsAvailable[i] != target_Main)
                {
                    target = targetsAvailable[i];
                    EnemyName.text = target.PlayerName;

                    cg.enabled = true;
                }

            Invoke("findTarget", 0.5f);
        }

        else
        {
            targetFound = true;
        }

    }

    private void FixedUpdate()
    {
        if (target)
        {
            HPCurrent = target.health;
            HPFull = target.fullHealth;
            HPSlider.fillAmount = HPCurrent / HPFull;

            /*
            if (HPCurrent >= 0)
                count.text = "HP: " + HPCurrent.ToString() + " / " + HPFull.ToString();
            else
                count.text = "HP: " + "0" + " / " + HPFull.ToString();
            */
        }

        else if (targetFound)
        {
            targetFound = false;
            cg.enabled = false;

            Invoke("findTarget", 0.5f);
        }
    }



    public void setTarget(PlayerBehaviour tg)
    {
        target = tg;
        HPFull = tg.health;
    }
}