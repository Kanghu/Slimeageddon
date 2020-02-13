using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RespawnCanvasScript : MonoBehaviour {

    public Button samuraiButton;
    public Button vikingButton;
    public Button rematchButton;
    public Button wizardButton;
    public Text selectionText;
    public int picked;
    public bool result;

	// Use this for initialization
	void Start () {
        samuraiButton.onClick.AddListener(delegate { ClassPicker(samuraiButton.name); });
        vikingButton.onClick.AddListener(delegate { ClassPicker(vikingButton.name); });
        wizardButton.onClick.AddListener(delegate { ClassPicker(wizardButton.name); });
        //rematchButton.onClick.AddListener(delegate { ChangeOrder(); });
    }

    void ClassPicker(string buttonName)
    {
        switch (buttonName)
        {
            case "Viking Respawn":
                picked = 0;
                selectionText.text = "Viking Selected";
                break;

            case "Samurai Respawn":
                picked = 1;
                selectionText.text = "Samurai Selected";
                break;

            case "Wizard Respawn":
                picked = 2;
                selectionText.text = "Wizard Selected";
                break;
        }
    }

    /*void ChangeOrder()
    {
        GetComponent<Canvas>().sortingOrder = -100;
    }*/

    // Update is called once per frame
    void Update () {
	
	}

}
