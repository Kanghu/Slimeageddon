using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestButtonScript : MonoBehaviour {

    // Use this for initialization
    public PlayerBehaviour[] targetsAvailable;

	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { dosmth(); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void dosmth()
    {
        targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

        for (int i = 0; i < targetsAvailable.Length; i++)
        {
            StartCoroutine(targetsAvailable[i].takeDamage(500));
        }

    }
}
