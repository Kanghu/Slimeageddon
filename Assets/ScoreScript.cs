using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    public PlayerBehaviour target;
    public int currScore;
    public int totalScore;

    // Use this for initialization
    void Start () {
        currScore = 0;
        totalScore = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (target)
        {
            if(target.myScore != currScore)
            {
                totalScore += (target.myScore - currScore);
                currScore = target.myScore;
            }

            GetComponent<Text>().text = totalScore.ToString();
        }
	}

    public void setTarget(PlayerBehaviour tg)
    {
        currScore = 0;
        target = tg;
    }
}
