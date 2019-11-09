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
    public float StunTimeCounting;

    public bool OnMove = false;
    public bool OnFollow = false;
    public bool OnStun;
    public bool IsWalkingLeft = false;

    public Rigidbody rb;

    public bool IsGrounded;
    public float groundCheckRange = 1f;
    public LayerMask groundLayer;



    //=======
    // Attack
    //=======
    public GameObject Target;


    // Start is called before the first frame update
    void Start()
    {
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
        }

        //=========
        // Movement
        //=========

        Direction = Target.transform.position - transform.position;

        if (OnFollow)
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

        

        //=========
        // Stun
        //=========

        if (StunTimeCounting < 1)
        {
            OnStun = true;
            StunTimeCounting += Time.deltaTime / StunTimerMax;
        }
        else if (StunTimeCounting >= 1)
        {
            OnStun = false;
        }

        //=========
        // Follow Check
        //=========

        if (OnStun)
        {
            OnFollow = false;
            EnemySprite.color = Color.red;
            
        }
        else
        {
            OnFollow = true;
            EnemySprite.color = Color.white;
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

    public void ReceiveBlow()
    {

    }

    public void ReceiveForce(float Force)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.left * transform.localScale.x * Force, ForceMode.Impulse);
    }


}


