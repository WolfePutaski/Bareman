using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : MonoBehaviour
{
    public LayerMask playerLayer;
    private Vector3 dir;
    public EnemyController Controller;

    public GameObject AttackHitbox;

    private float ChargeTimerCounting = 1f;
    private float AttackTimerCounting = 1f;
    private float AttackDelayTimerCounting = 1f;

    public GameObject Target;

    public float DetectRange = 1.5f;
    public float AttackChargeTime;
    public float AttackTime;
    public float AttackDelayTime;

    public int AttackDamage = 1;
    public float AttackDash = 1f;
    public float AttackPushForce = 10f;

    public bool OnChecking = true;
    public bool OnCharging = false;
    public bool OnAttacking = false;

    public int State; //0-Get Close; 1-Get medium; 2-GetFar 
    public float KeepDistance;


    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("PlayerCollider");
        //Player = GameObject.Find("PlayerCollider");

        AttackHitbox.SetActive(false);
        ChargeTimerCounting = 1f;
        AttackTimerCounting = 1f;  

    }   

    // Update is called once per frame
    void Update()
    {

        //===============
        //Color Debug
        //===============
        {
            if (OnCharging == true)
            {
                Controller.EnemySprite.color = new Color(0f, 255f, 0f);
            }
            else if (!Controller.OnStun)
            {
                Controller.EnemySprite.color = Color.white;
            }
        }

        //===============
        //State
        //===============
        if (State == 0) //Moving to attack
        {
            Controller.OnFollow = true;
        }
        else if (State != 0)
        {
            Controller.OnFollow = false;
        }

        if (!OnCharging || !OnAttacking || !Controller.OnStun)
        {
            if (State == 1)
            {
                transform.localPosition = Vector3.MoveTowards(transform.position, Target.transform.position + Vector3.right * KeepDistance * Controller.transform.localScale.x, Controller.WalkSpeed * Time.deltaTime);
            }
        }
        //===============
        //Attack Detection
        //===============

        dir = Vector3.Normalize(Controller.Direction);
        Vector3 attackdir = new Vector3(transform.localScale.x, 0, 0);

        Debug.DrawRay(transform.position, attackdir * DetectRange, Color.cyan); //Debug Raycast

        if (OnChecking)
        {

                if (Physics.Raycast(transform.position, attackdir + Vector3.right * DetectRange * transform.localScale.x , DetectRange, playerLayer) || AttackDelayTimerCounting < 1)
                {
                    ChargeTimerCounting = 0f;
                    OnCharging = true;
                    AttackDelayTimerCounting = 0;
                }

        }

        if (OnCharging || OnAttacking || Controller.OnStun)
        {
            OnChecking = false;
            Controller.OnFollow = false;
        }
        else if (!Controller.OnStun)
        {
            OnChecking = true;
            Controller.OnFollow = true;
        }


        if (Controller.OnStun)
        {
            ChargeTimerCounting = -1;
            OnCharging = false;
        }

        //===============
        //Charging
        //===============

        if (OnCharging == true)
        {
            if (ChargeTimerCounting < 1)
            {
                //Controller.rb.mass = 999;
                ChargeTimerCounting += Time.deltaTime / AttackChargeTime;
            }
            else
            {
                //Controller.rb.mass = 5;


                GetComponent<Rigidbody>().AddForce(attackdir * AttackDash, ForceMode.VelocityChange); //Attack Push
                AttackTimerCounting = 0f;
                OnAttacking = true;
                OnCharging = false;
            }

        }

        //===============
        //Attacking
        //===============

        if (transform.localScale.x < 0 && AttackPushForce > 0)
        {
            AttackPushForce = -AttackPushForce;

        }
        else if (transform.localScale.x > 0 && AttackPushForce < 0)
        {
            AttackPushForce = -AttackPushForce;
        }


        if (OnAttacking == true)
        {


            if (AttackTimerCounting < 1)
            {
                AttackHitbox.SetActive(true);
                AttackTimerCounting += Time.deltaTime / AttackTime;
            }


            else
            {
                OnAttacking = false;
                AttackHitbox.SetActive(false);
                Controller.OnFollow = true;
            }

            
        }

        if (AttackDelayTimerCounting < 1)
        {
            OnChecking = false;
            AttackDelayTimerCounting += Time.deltaTime / AttackDelayTime;
        }


        //===============
        //Hitbox
        //===============

    }

    private void OnTriggerEnter(Collider other)
    {
        if (AttackHitbox && other.tag == "Player")
        {
            other.SendMessage("ReceiveDamage", AttackDamage);
            other.SendMessage("ReceiveForce", AttackPushForce);

            Debug.Log("Taken Damage");
        }
    }
}
