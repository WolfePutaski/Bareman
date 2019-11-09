using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Player;

    public int HP = 5;

    public float WalkSpeed = 300.5f;
    public float RollSpeed = 300.5f;
    public float RollTabInterval = 0.1f;
    public float MaxInvincibleTime = 0.5f;
    public float InvincibleTime = 1;

    public Vector3 MoveDirection;
    public Rigidbody rb;
    public Vector3 Velocity;

    public bool IsGrounded;
    public float groundCheckRange = 1f;
    public LayerMask groundLayer;

    //public float xRaw;
    //public float zRaw;

    public bool IsWalkingLeft = false;
    public bool IsWalkingDown = false;
    //public bool ToRoll = false;
    public bool OnRoll = false;
    public bool OnRollRegen = false;
    public bool IsInvincible = false;

    public float RollTime = 1f;
    public float MaxRollRegenTime = 5f;
    public float RollRegenTimeCount;
    public int RollCount = 3;
    public int MaxRollCount = 3;

    public float JumpForce = 50f;


    public SpriteRenderer PlayerSprite;



    // Start is called before the first frame update
    void Start()
    {
        //AttackHitBox.SetActive(false);
        RollCount = MaxRollCount;
        RollRegenTimeCount = 1;
    }

    // Update is called once per frame

    void Update()
    {
        //=========
        // Grounded
        //=========

        IsGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckRange, groundLayer);

        if (IsGrounded)
        {
            rb.drag = 12;
        }
        else if (!IsGrounded)
        {
            rb.drag = 0;
        }

        //=========
        // Walking & Rolling
        //=========

        if (GetComponent<PlayerAttack>().IsAttacking == false && GetComponent<PlayerAttack>().IsTired == false && IsGrounded) // Moving Condition
        {
            Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //xRaw = Input.GetAxisRaw("Horizontal");
            //zRaw = Input.GetAxisRaw("Vertical");
            GetComponent<Rigidbody>().AddForce(Direction * WalkSpeed * Time.deltaTime, ForceMode.VelocityChange);
            //ToRoll = true;
            //StartCoroutine("SetTabDelay", RollTabInterval);


            if (RollCount > 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && OnRoll == false)
                {
                    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                    {
                        Vector3 RollDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                        RollCount--;
                        RollRegenTimeCount = 0;
                        OnRoll = true;
                        //ToRoll = false;
                        GetComponent<Rigidbody>().AddForce(RollDirection * RollSpeed, ForceMode.Impulse);
                        StartCoroutine("RollDelay", RollTime);
                    }
                }
            }

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

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

            //}
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

        if (OnRoll)
        {
            gameObject.tag = "PlayerDodge";
            gameObject.layer = 2;

        }

        //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) 
        //    && OnRoll == false && GetComponent<Rigidbody>().velocity.magnitude > 0 && GetComponent<PlayerAttack>().IsAttacking == false)
        ////{
        ////    if (ToRoll == true)
        ////    {
        ////        Vector3 RollDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        ////        RollCount--;
        ////        RollRegenTimeCount = 0;
        ////        OnRoll = true;
        ////        ToRoll = false;
        ////        GetComponent<Rigidbody>().AddForce(RollDirection * RollSpeed, ForceMode.Impulse);
        ////        StartCoroutine("RollDelay", RollTime);
        ////    }
        ////    else
        //{
        //    ToRoll = true;
        //    StartCoroutine("SetTabDelay", RollTabInterval);
        //}
        ////}

        Velocity = GetComponent<Rigidbody>().velocity;

        //Regen

        if (RollCount < MaxRollCount)
        {
            OnRollRegen = true;
        }

        if (OnRollRegen)
        {
            if (RollRegenTimeCount < 1)
            {
                RollRegenTimeCount += Time.deltaTime / MaxRollRegenTime;
            }
            else
            {
                RollCount++;
                RollRegenTimeCount = 0;
                OnRollRegen = false;
            }
        }

        if(InvincibleTime < 1)
        {
            IsInvincible = true;
            InvincibleTime += Time.deltaTime / MaxInvincibleTime;
        }
        else
        {
            IsInvincible = false;
        }

        if (IsInvincible || OnRoll)
        {
            gameObject.tag = "PlayerDodge";
            gameObject.layer = 2;
            PlayerSprite.color = Color.gray;


        }
        else
        {
            gameObject.tag = gameObject.tag = "Player";
            gameObject.layer = 8;
            PlayerSprite.color = Color.white;

        }
    }

    public void ReceiveDamage(int Damage)//, float Timer)
    {
        HP -= Damage;
        InvincibleTime = 0;
        //   StunTimerMax = Timer;
        // Stun();
        if (HP <= 0)
        {
            //if (IsBoss)
            //{
            //    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsWon = true;
            //}
            Destroy(this.gameObject);
        }
    }

    public void ReceiveForce(float Force)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * Force, ForceMode.Impulse);
    }


    //IEnumerator SetTabDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    ToRoll = false;

    //}

    IEnumerator RollDelay(float delay)
    {
        //rb.velocity = Vector3.zero;
        //Vector3 dir = new Vector3(xRaw, 0, zRaw);

        //rb.velocity += dir.normalized * RollSpeed;
        yield return new WaitForSeconds(delay);
        OnRoll = false;
        gameObject.tag = "Player";
        gameObject.layer = 8;

    }
}