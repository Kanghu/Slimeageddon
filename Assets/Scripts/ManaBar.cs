using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaBar : MonoBehaviour {

    public Text count;
    public Image ManaSlider;
    public PlayerBehaviour target;
    private Image FillVal;


    //Color:
    public int PlayerIndex;
    public Color[] Color_BG;
    public Color[] Color_Fill;
    public Sprite[] Image_Bar;


    public float ManaFull;
    public float ManaCurrent;

    private void ChangeAspect(int i)
    {
        GameObject.Find("Player UI's/Player UI/Mana/Mana BG").GetComponent<Image>().color = Color_BG[i];
        GameObject.Find("Player UI's/Player UI/Mana/Fill").GetComponent<Image>().color = Color_Fill[i];
        GameObject.Find("Player UI's/Player UI/Mana/Overlay").GetComponent<Image>().sprite = Image_Bar[i];
    }


    private void FixedUpdate()
    {
        if (target)
        {
            ManaCurrent = target.mana;
            ManaSlider.fillAmount = ManaCurrent / ManaFull;
            if (ManaCurrent >= 0)
                count.text = "Mana: " + ((int)ManaCurrent).ToString();
            else
                count.text = "Mana: " + "0";
        }
    }

    public void setTarget(PlayerBehaviour tg)
    {
        target = tg;
        ManaFull = tg.mana;

        ChangeAspect(tg.playerNr - 1);
    }
}
