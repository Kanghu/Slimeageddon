using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CDBar : MonoBehaviour
{
   

    public Text[] count = new Text[4];
    public Image[] CDSlider = new Image[4];
    public CooldownBH target;
    //private Image[] FillVal = new Image[4];


    public float[] CDFull = new float[4];
    public float[] CDCurrent = new float[4];

    

    private void FixedUpdate()
    {
        if (target)
        {
            ///Mouse
             CDCurrent[0] = Time.time - target.Start_M2;
             CDFull[0] = target.Cooldown_M2;

             CDSlider[0].fillAmount = 1 - CDCurrent[0] / CDFull[0];


            ///Q
            CDCurrent[1] = Time.time - target.Start_Q;
            CDFull[1] = target.Cooldown_Q;

            CDSlider[1].fillAmount = 1 - CDCurrent[1] / CDFull[1];


            ///E
            CDCurrent[2] = Time.time - target.Start_E;
            CDFull[2] = target.Cooldown_E;

            CDSlider[2].fillAmount = 1 - CDCurrent[2] / CDFull[2];



            /*
            if (CDCurrent[0] >= 0)
                count[0].text = "Mana: " + ((int)CDCurrent[0]).ToString();
            else
                count[0].text = "Mana: " + "0";

            */



            /*
            CDCurrent = target.cooldown;
            CDSlider.fillAmount = CDCurrent / CDFull;
            if (CDCurrent >= 0)
                count.text = "Mana: " + ((int)CDCurrent).ToString();
            else
                count.text = "Mana: " + "0";

            */
        }
    }

    public void setTarget(CooldownBH tg)
    {
        target = tg;
       
    }
}
