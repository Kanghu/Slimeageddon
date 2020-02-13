using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float dampTime = 0.15f;
    public bool followParent = false;
    private Vector3 velocity = Vector3.zero;
    public Transform target = null;

    public bool border;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    private void Start()
    {
        if (followParent)
        {
            //setTarget(GetComponentInParent<RectTransform>());
            GetComponent<Transform>().position = new Vector3 (GetComponentInParent<RectTransform>().position.x, GetComponentInParent<RectTransform>().position.y, GetComponent<Transform>().position.z);
          //  GetComponent<Camera>().enabled = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            /* if(transform.position > 0)
             {

             }*/

            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); // (new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        if(border && !followParent)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
        }

       /* else if ( GameObject.FindWithTag("Player 1") ) //cauta Player 1  --- *de schimbat*
            target = GameObject.FindGameObjectWithTag("Player 1").transform ;*/

    }

    public void setTarget(Transform tg)
    {
        target = tg;
    }
    

    public void findTarget()
    {
        if(target == null)
        {
            Debug.Log( Network.connections.Length );

            Invoke("FindTarget", 0.5f);
        }
    }
}
