using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool CanFollow = true;

    public bool FollowPlayer = true;
    public GameObject Player;
    public GameObject WallFront;
    public Collider PlayerCol;
    public Collider WallFrontCol;

    public PlayerController PlayerController;

    public Vector3 Position;

    public float MaxOffsetX;
    public float NewMaxOffsetX;
    public Vector3 offset;            //Private variable to store the offset distance between the player and camera



    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("Player");
        PlayerController = Player.GetComponent<PlayerController>();
        NewMaxOffsetX = Mathf.Abs(offset.x);

        PlayerCol = Player.GetComponent<Collider>();
        WallFrontCol = WallFront.GetComponent<Collider>();

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

            //if (Mathf.Abs(offset.x) <= MaxOffsetX)
            FollowPlayer = true;

            WallFront.SetActive(PlayerController.RollCount == PlayerController.MaxRollCount);

            //if (offset.x > MaxOffsetX)
            //{
            //    transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x - MaxOffsetX, transform.position.y, transform.position.z), offset.x*Time.deltaTime);
            //}

        }

        else if (CanFollow == false)
        {
            FollowPlayer = false;
            if (Player.transform.position.x < WallFront.transform.position.x - 0.5)
            {
                WallFront.SetActive(true);
            }
            else
            {
                Player.transform.position = new Vector3(WallFront.transform.position.x -0.5f
                    , Player.transform.position.y, Player.transform.position.z);
            }
        }


    }

    void FixedUpdate()
    {
        if (FollowPlayer)
        {
            if (Mathf.Abs(offset.x) > MaxOffsetX)
            {
                if (offset.x >= MaxOffsetX)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Player.transform.position.x - MaxOffsetX, transform.position.y, transform.position.z), offset.x * Time.deltaTime * 0.95f);
                    //transform.position = new Vector3(Player.transform.position.x - MaxOffsetX, transform.position.y, transform.position.z);
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
