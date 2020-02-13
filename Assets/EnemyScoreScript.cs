using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScoreScript : MonoBehaviour {

    public bool targetFound = false;

    public PlayerBehaviour target;
    public PlayerBehaviour target_Main;
    public PlayerBehaviour[] targetsAvailable;

    public int currScore;
    public int totalScore;

    private void Start()
    {
        currScore = 0;
        totalScore = 0;
        Invoke("findTarget", 0.5f);
    }

    private void findTarget()
    {

        if (target_Main == null)
        {
            target_Main = GameObject.Find("My Score").GetComponent<ScoreScript>().target;
        }

        if (target == null)
        {
            targetsAvailable = FindObjectsOfType<PlayerBehaviour>();

            for (int i = 0; i < targetsAvailable.Length; i++)
                if (targetsAvailable[i] != target_Main)
                {
                    target = targetsAvailable[i];
                    currScore = 0;
                }

            Invoke("findTarget", 0.5f);
        }

        else
        {
            targetFound = true;
        }

    }
    // Update is called once per frame
    void Update () {

        if (target)
        {
            if (target.myScore != currScore)
            {
                totalScore += (target.myScore - currScore);
                currScore = target.myScore;
            }

            GetComponent<Text>().text = totalScore.ToString();
        }

        else if (targetFound)
        {
            targetFound = false;

            Invoke("findTarget", 0.5f);
        }
    }

    public void setTarget(PlayerBehaviour tg)
    {
        target = tg;
    }
}
