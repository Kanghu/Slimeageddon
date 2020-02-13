using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayButtonScript : MonoBehaviour {

    public Button play;
	// Use this for initialization
	void Start () {
        play.onClick.AddListener(delegate { disableThis(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void disableThis()
    {
        gameObject.SetActive(false);
    }
}
