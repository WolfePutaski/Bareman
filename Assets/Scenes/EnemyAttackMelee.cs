using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : MonoBehaviour
{
    public LayerMask playerLayer;
    private Vector3 dir;
    public EnemyController Controller;
    public PlayerController Player;

    public GameObject AttackHitbox;

    public float ChargeTimerCounting = 1f;
    public float AttackTimerCounting = 1f;

    public GameObject Target;

    public float DetectRange = 1.5f;
    public float AttackChargeTime;
    public float AttackTime;

    public int AttackDamage = 1;
    public float AttackPushForce = 1f;

    public bool OnChecking = true;
    public bool OnCharging = false;
    public bool OnAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 attackdir = new Vector3(Controller.transform.localScale.x, 0, 0);

        Debug.DrawRay(transform.position, attackdir * DetectRange, Color.cyan); //Debug Raycast

        if (OnChecking)
        {
            if (Player.OnRoll == false)
            {
                if (Physics.Raycast(transform.position, attackdir , DetectRange, playerLayer))
                {
                    ChargeTimerCounting = 0f;
                    OnCharging = true;
                }
            }
           
        }

        if (OnCharging || OnAttacking)
        {
            OnChecking = false;
            Controller.OnFollow = false;
        }
        else
        {
            OnChecking = true;
            Controller.OnFollow = true;
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
                GetComponent<Rigidbody>().AddForce(dir * AttackPushForce, ForceMode.Impulse); //Attack Push
                AttackTimerCounting = 0f;
                OnAttacking = true;
                OnCharging = false;
            }

        }

        //===============
        //Attacking
        //===============

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
            Debug.Log("Taken Damage");
        }
    }
}
