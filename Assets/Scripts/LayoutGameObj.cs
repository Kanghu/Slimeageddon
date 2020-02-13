using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[ExecuteInEditMode]
public class LayoutGameObj : MonoBehaviour {


    public float distUp, distDown, distLeft, distRight;

    public bool applySize = false;
    public float width, height;

    public bool onlyStart;

    float min;
    
    RectTransform parent;
    GridLayoutGroup grid;

    void Start()
    {
        if (onlyStart)
        {
            
            if (applySize)
                GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height * width / 100, Screen.height * height / 100);
            else
            {
                GetComponent<RectTransform>().offsetMin = new Vector2(-Screen.height * distLeft / 100, -Screen.height * distDown / 100);
                GetComponent<RectTransform>().offsetMax = new Vector2(Screen.height * distRight / 100, Screen.height * distUp / 100);
            }
        }
    }


    void Update()
    {
        if (!onlyStart)
        {
            //  parent.localPosition = new Vector3(0f, -Screen.height / 20, 0);
            if (applySize)
                GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height * width / 100, Screen.height * height / 100);
            else
            {
                GetComponent<RectTransform>().offsetMin = new Vector2(-Screen.height * distLeft / 100, -Screen.height * distDown / 100);
                GetComponent<RectTransform>().offsetMax = new Vector2(Screen.height * distRight / 100, Screen.height * distUp / 100);
            }
        }

    }
}
