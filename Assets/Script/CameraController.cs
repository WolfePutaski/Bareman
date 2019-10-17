using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool FollowPlayer = true;
    public Transform Player;

    public Vector3 Position;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (FollowPlayer)
        {
            new Vector3(Player.transform.position.x,transform.position.y,transform.position.z);
        }
    }
}
