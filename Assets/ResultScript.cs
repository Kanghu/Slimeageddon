using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ResultScript : MonoBehaviour {

    public bool won;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        won = GameObject.Find("Respawn Canvas").GetComponent<RespawnCanvasScript>().result;

        if (won)
        {
            GetComponent<Text>().text = "You won";
        }

        else
        {
            GetComponent<Text>().text = "You lost";
        }
	}
}
