using UnityEngine;
using System.Collections;

public class FixCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<BoxCollider2D>().size = new Vector2( (float) Screen.width / 88, (float) Screen.height / 88);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
