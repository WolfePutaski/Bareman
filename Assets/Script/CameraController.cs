using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool CanFollow = true;

    public bool FollowPlayer = true;
    public Transform Player;

    public Vector3 Position;

    public float MaxOffsetX;
    public float NewMaxOffsetX;
    public Vector3 offset;            //Private variable to store the offset distance between the player and camera



    // Start is called before the first frame update
    void Start()
    {
        offset = Player.transform.position - transform.position;

        NewMaxOffsetX = Mathf.Abs(offset.x);
    }

    // Update is called once per frame
    void Update()
    {
        offset = Player.transform.position - transform.position;

        Position = transform.position;

        if (Mathf.Abs(offset.x) > MaxOffsetX)
        {
            NewMaxOffsetX = Mathf.Abs(offset.x);
        }

        if (CanFollow)

        {

            if (Mathf.Abs(offset.x) <= MaxOffsetX)
            FollowPlayer = true;

            if (offset.x > MaxOffsetX)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Player.transform.position.x - MaxOffsetX, transform.position.y, transform.position.z), Time.deltaTime);
            }

        }

        else if (CanFollow == false)
        {
            FollowPlayer = false;
        }

        if (FollowPlayer)
        {
            if (Mathf.Abs(offset.x) > MaxOffsetX)
            {
                if (offset.x >= MaxOffsetX)
                {
                    transform.position = new Vector3(Player.transform.position.x - MaxOffsetX, transform.position.y, transform.position.z);
                }
                //if (offset.x <= -MaxOffsetX)
                //{
                //    transform.position = new Vector3(Player.transform.position.x + MaxOffsetX, transform.position.y, transform.position.z);
                //}
            }

            //if (NewMaxOffsetX > MaxOffsetX)
            //{

            //if (offset.x >= NewMaxOffsetX)
            //{
            //    transform.position = new Vector3(Player.transform.position.x - NewMaxOffsetX, transform.position.y, transform.position.z);
            //}
            //if (offset.x <= -NewMaxOffsetX)
            //{
            //    transform.position = new Vector3(Player.transform.position.x + NewMaxOffsetX, transform.position.y, transform.position.z);
            //}
            //}

        }
    }
}
