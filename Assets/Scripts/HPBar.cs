using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBar : MonoBehaviour
{
    public Text count;
    public Image HPSlider;
    public PlayerBehaviour target;
    private Image FillVal;

    public float HPFull;
    public float HPCurrent;
  

  

    private void FixedUpdate()
    {
        if (target)
        {
            HPCurrent = target.health;
            HPFull = target.fullHealth;
            HPSlider.fillAmount = HPCurrent / HPFull;

            /*
             
            if (HPCurrent >= 0)
                count.text = "HP: " + HPCurrent.ToString() + " / " + HPFull.ToString() ;
            else
                count.text = "HP: " + "0" + " / " + HPFull.ToString();

            */
        }
    }

    public void setTarget(PlayerBehaviour tg)
    {
        target = tg;
        HPFull = tg.health;
    }
}