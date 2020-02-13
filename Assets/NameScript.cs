using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameScript : MonoBehaviour {

    public PlayerBehaviour target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<TextMesh>().text = target.PlayerName;

        if (!target.faceRight)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            GetComponent<TextMesh>().anchor = TextAnchor.UpperRight;
        }
        else if (target.faceRight)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
        }
    }
}
