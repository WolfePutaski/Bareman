using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //when stumble = No Follow, No Move, No Attack
    // Controller: Sense: Brain;

    public SpriteRenderer EnemySprite;
    
    public float HP = 10f;
    public float WalkSpeed = 300.5f;

    public Vector3 Direction;

    public float StunType; //For different stun type
    public float StunTimerMax;

    public float StunStaggerTimerMax;
    public float StunKnockdownTimerMax;

    private float StunTimeCounting;

    public bool OnFollow = false;
    public bool OnStun;
    public bool IsWalkingLeft = false;
    public bool IsOutside;

    public Rigidbody rb;

    public bool IsGrounded;
    public float groundCheckRange;
    public LayerMask groundLayer;

    public float MinRange = 3f;
    public GameObject Target;
    public GameObject Camera;


    public GameObject[] Walls;
    public GameObject WallFront;
    public GameObject WallBack;


    public float OnScreenXPos;

    private Vector3 DefaultCenter;
    private float DefaultHeight;

    public CapsuleCollider col;


    void Awake()
    {
        groundCheckRange = transform.position.y + 0.1f;

        StunTimeCounting = 1f;
        Target = GameObject.Find("Player");
        Camera = GameObject.Find("Main Camera");

        col = gameObject.GetComponent<CapsuleCollider>();

        Walls = GameObject.FindGameObjectsWithTag("Wall");
        DefaultCenter = col.center;
        DefaultHeight = col.height;

        WallFront = GameObject.Find("WallFront");
        WallBack = GameObject.Find("WallBack");

}

    // Update is called once per frame
    void Update()
    {

        ////=========
        //// Wall Collider
        ////=========

        //IsOutside = (Mathf.Abs(OnScreenPos) > 5);
        //OnScreenPos = Mathf.Abs(transform.position.x) - Camera.transform.position.x;

        //Collider col = gameObject.GetComponent<Collider>();
        //    for (int i = 0; i < Walls.Length; i++)
        //    {
        //        Collider wall = Walls[i].GetComponent<Collider>();
        //        Physics.IgnoreCollision(col, wall, (!IsOutside));
        //    }


        Physics.IgnoreCollision(col, WallFront.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(col, WallBack.GetComponent<Collider>(), true);

        IsOutside = (Mathf.Abs(transform.position.x) > WallFront.transform.position.x - 1.5f);


        if (IsOutside && !OnStun)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Camera.transform.position.x, transform.position.y, transform.position.z), WalkSpeed * Time.deltaTime);
        }

        //=========
        // Grounded
        //=========
        IsGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckRange, groundLayer);

        if (IsGrounded)
        {
            rb.drag = 12;
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        }
        else if (!IsGrounded)
        {
            rb.drag = 0;
            OnFollow = false;
        }

        //=========
        // Movement
        //=========

        Direction = new Vector3( Target.transform.position.x - transform.position.x, transform.position.y, Target.transform.position.z - transform.position.z);

        if (OnFollow && !IsOutside)
        {

            //if (Mathf.Abs(Direction.z) <= MinRange && Mathf.Abs(Direction.x) <= MinRange)
            //{
            //    transform.position = Vector3.MoveTowards(transform.position , new Vector3(Target.transform.position.x - (MinRange * transform.localScale.normalized.x), transform.position.y, Target.transform.position.z), WalkSpeed * Time.deltaTime);
            //}
            //else
            //{

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Target.transform.position.x - (MinRange * transform.localScale.normalized.x), transform.position.y, Target.transform.position.z), WalkSpeed * Time.deltaTime);
            //if (Mathf.Abs(Target.transform.position.x - transform.position.x) > MinRange)
            //rb.AddForce(new Vector3(Mathf.Round(Direction.normalized.x), 0, Mathf.Round(Direction.normalized.z)) * WalkSpeed, ForceMode.VelocityChange);
            //else { }
            //}


        }


        //=========
        // Inside Wall
        //=========


        //=========
        // Turninng
        //=========

        if (IsWalkingLeft == false && Direction.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            IsWalkingLeft = true;
        }
        else if (IsWalkingLeft == true && Direction.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            IsWalkingLeft = false;

        }





        //=========
        // Stun
        //=========
        if (StunType == 0)
        {
            StunTimerMax = StunStaggerTimerMax;

            if (StunTimeCounting < 1)
            {

                EnemySprite.color = Color.yellow;
                OnStun = true;
                StunTimeCounting += Time.deltaTime / StunTimerMax;

            }

        }

        if (StunType == 1)
        {

            StunTimerMax = StunKnockdownTimerMax;
            if (StunTimeCounting < 1)
            {
                EnemySprite.color = Color.red;
                EnemySprite.transform.localPosition = new Vector3(0, -1.3f, 0);

                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, -0.7f, 0);
                gameObject.GetComponent<CapsuleCollider>().height = gameObject.GetComponent<CapsuleCollider>().radius/2;

                OnStun = true;
                StunTimeCounting += Time.deltaTime / StunTimerMax;

            }
            else if (StunTimeCounting >= 1)
            {
                EnemySprite.transform.localPosition = new Vector3(0,0, 0);
                gameObject.GetComponent<CapsuleCollider>().center = DefaultCenter;
                gameObject.GetComponent<CapsuleCollider>().height = DefaultHeight;

            }
        }

        if (OnStun && StunTimeCounting >= 1)
        {
            OnFollow = true;
            OnStun = false;
            //enemysprite.color = color.white;
        }
        //=========
        // Follow Check
        //=========

        if (OnStun)
        {
            OnFollow = false;
            
        }

    }
     
    //=========
    // Health
    //=========

    public void ReceiveDamage(int Damage)//, float Timer)
    {
        //Deactive when receiving damage.

        HP -= Damage;
        StunTimeCounting = 0;

        if (HP <= 0)
        {
            //if (IsBoss)
            //{
            //    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsWon = true;
            //}
            Destroy(this.gameObject);
        }
        //MeleeAttackController.ChargeTimerCounting = -1;
        //RangedAttackController.ChargeTimerCounting = -1;
        //MeleeAttackController.OnCharging = false;
        //RangedAttackController.OnCharging = false;
    }

    //=========
    // On Stun
    //=========
    
    public void Stun()
    {
        StunTimeCounting = 0;
    }


    public void ReceiveForce(float Force)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * Target.transform.localScale.x * Force, ForceMode.Impulse);
    }

    public void ReceiveBlow(int BlowType)
    {
        StunType = BlowType;
    }

}


