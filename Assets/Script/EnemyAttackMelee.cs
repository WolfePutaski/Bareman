using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : MonoBehaviour
{
    public LayerMask playerLayer;
    private Vector3 dir;
    public EnemyController Controller;

    public GameObject AttackHitbox;

    public float ChargeTimerCounting = 1f;
    public float AttackTimerCounting = 1f;

    public GameObject Target;

    public float DetectRange = 1.5f;
    public float AttackChargeTime;
    public float AttackTime;

    public int AttackDamage = 1;
    public float AttackDash = 1f;
    public float AttackPushForce = 10f;

    public bool OnChecking = true;
    public bool OnCharging = false;
    public bool OnAttacking = false;


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
        //Attack Detection
        //===============

        dir = Vector3.Normalize(Controller.Direction);
        Vector3 attackdir = new Vector3(transform.localScale.x, 0, 0);

        Debug.DrawRay(transform.position, attackdir * DetectRange, Color.cyan); //Debug Raycast

        if (OnChecking)
        {

                if (Physics.Raycast(transform.position, attackdir , DetectRange, playerLayer))
                {
                    ChargeTimerCounting = 0f;
                    OnCharging = true;
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
                ChargeTimerCounting += Time.deltaTime / AttackChargeTime;
            }
            else
            {
                GetComponent<Rigidbody>().AddForce(dir * AttackDash, ForceMode.Impulse); //Attack Push
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
