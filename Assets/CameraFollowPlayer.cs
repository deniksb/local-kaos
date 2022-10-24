using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    
    public GameObject player;

    public Vector3 offset = new Vector3(0, 0, -1);

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player)
        {
            transform.position = new Vector3(
                player.transform.position.x + offset.x,
                player.transform.position.y + offset.y,
                player.transform.position.z + offset.z);
        }
    }
}
