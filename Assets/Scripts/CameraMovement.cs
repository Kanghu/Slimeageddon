using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    //1.51 - 650

    public Transform target;
    Camera mycam;
	// Use this for initialization
	void Start () {
        mycam = GetComponent<Camera>();
        mycam.orthographicSize = Screen.height / 365f * 1.5f;
    }
	
	// Update is called once per frame
	void Update () {
        

        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.5f) + new Vector3(0, 0.3f * Screen.height / 365, -0.5f);

        }
    }
}
