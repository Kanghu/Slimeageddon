using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") / 10;
        var z = Input.GetAxis("Vertical") / 10;

        transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y + z, transform.localPosition.z);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(transform); // propria camera urmareste propriul caracter
    }
}