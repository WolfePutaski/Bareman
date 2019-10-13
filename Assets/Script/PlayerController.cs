using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float WalkSpeed = 300.5f;
    public float RollSpeed = 300.5f;
    public float RollTabInterval = 0.1f;
    public float RollTime = 1f;
    public Vector3 MoveDirection;
    public Rigidbody rb;

    public float xRaw;
    public float zRaw;

    public bool IsWalkingLeft = false;
    public bool IsWalkingDown = false;
    public bool ToRoll = false;
    public bool OnRoll = false;


    // Start is called before the first frame update
    void Start()
    {
        //AttackHitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //=========
        // Walking & Rolling
        //=========
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        xRaw = Input.GetAxisRaw("Horizontal");
        zRaw = Input.GetAxisRaw("Vertical");
        GetComponent<Rigidbody>().AddForce(Direction * WalkSpeed * Time.deltaTime, ForceMode.VelocityChange);


        if (Direction.x < 0)
        {
            //PlayerSprite.flipX = true;
            IsWalkingLeft = true;
        }
        else if (Direction.x > 0)
        {
            //PlayerSprite.flipX = false;
            IsWalkingLeft = false;
        }
        if (Direction.z < 0)
        {
            IsWalkingDown = true;
        }
        else if (Direction.z > 0)
        {
            IsWalkingDown = false;
        }

        if (OnRoll)
        {
            Direction = new Vector3(xRaw, 0, zRaw);

            GetComponent<Rigidbody>().AddForce(Direction * RollSpeed, ForceMode.Impulse);

        }

        //=========
        // Turninng
        //=========
        if (IsWalkingLeft == true && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (IsWalkingLeft == false && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //=========
        // Rolling
        //=========

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) && OnRoll == false)
        {
            if (ToRoll == true)
            {
                OnRoll = true;
                ToRoll = false;
                StartCoroutine("RollDelay", RollTime);

            }
            else
            {
                ToRoll = true;
                StartCoroutine("SetTabDelay", RollTabInterval);
            }
        }

        



    }

    //private void Roll(float x, float z)
    //{
    //    rb.velocity = Vector3.zero;
    //    Vector3 dir = new Vector3(x, 0, z);

    //    rb.velocity += dir.normalized * RollSpeed;
    //    StartCoroutine("OnRollTime", RollTime);

    //}
    

    IEnumerator SetTabDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToRoll = false;

    }

    IEnumerator RollDelay(float delay)
    {
        //rb.velocity = Vector3.zero;
        //Vector3 dir = new Vector3(xRaw, 0, zRaw);

        //rb.velocity += dir.normalized * RollSpeed;
        yield return new WaitForSeconds(delay);
        OnRoll = false;
    }


}