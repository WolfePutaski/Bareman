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
    public Vector3 rangeTransform;


    public float StunType; //For different stun type
    public float StunTimerMax;

    public float StunStaggerTimerMax;
    public float StunKnockdownTimerMax;

    public float StunTimeCounting;

    public bool OnMove = false;
    public bool OnFollow = false;
    public bool OnStun;
    public bool IsWalkingLeft = false;

    public Rigidbody rb;

    public bool IsGrounded;
    public float groundCheckRange;
    public LayerMask groundLayer;

    public bool IsRanged;
    public float MinRange = 3f;



    //=======
    // Attack
    //=======
    public GameObject Target;


    // Start is called before the first frame update
    void Start()
    {
        groundCheckRange = transform.position.y + 0.1f;

        StunTimeCounting = 1f;
        Target = GameObject.Find("Player");
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
            OnFollow = false;
        }

        //=========
        // Movement
        //=========

        rangeTransform = new Vector3(Target.transform.position.x - (MinRange * Direction.normalized.x), Target.transform.position.y, Target.transform.position.z);
        rangeTransform.y = 0;


        Direction = Target.transform.position - transform.position;
        Direction.y = 0;

        if (OnFollow)
        {
            if (IsRanged)
            {

                transform.position = Vector3.MoveTowards(transform.position, rangeTransform, WalkSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, WalkSpeed * Time.deltaTime);

            }


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
            else if (StunTimeCounting >= 1)
            {
                OnStun = false;
            }
        }

        if (StunType == 1)
        {
            StunTimerMax = StunKnockdownTimerMax;
            if (StunTimeCounting < 1)
            {
                EnemySprite.color = Color.red;

                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0,-1.8f,0);
                OnStun = true;
                StunTimeCounting += Time.deltaTime / StunTimerMax;

            }
            else if (StunTimeCounting >= 1)
            {
                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0f, 0);

                OnStun = false;

            }
        }

        //=========
        // Follow Check
        //=========

        if (OnStun)
        {
            OnFollow = false;
            
        }
        else
        {
            OnFollow = true;
            //enemysprite.color = color.white;
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
        GetComponent<Rigidbody>().AddForce(Vector3.right * Target.transform.localScale.x * Force, ForceMode.VelocityChange);
    }

    public void ReceiveBlow(int BlowType)
    {
        StunType = BlowType;
    }

}


