using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRanged : MonoBehaviour
{
    public LayerMask playerLayer;
    private Vector3 dir;
    public EnemyController Controller;

    public GameObject Projectile;

    public float ChargeTimerCounting = 1f;
    public float AttackTimerCounting = 1f;

    public GameObject Target;

    public float DetectRange = 5f;
    public float AttackChargeTime;
    public float AttackTime;

    public int AttackDamage = 1;
    public float AttackPushForce = 10f;

    public bool OnChecking = true;
    public bool OnCharging = false;
    public bool OnAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        //Target = GameObject.Find("PlayerCollider");

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
     
                if (Physics.Raycast(transform.position, attackdir, DetectRange, playerLayer))
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
        else if(!Controller.OnStun)
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
                Fire();

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
                AttackTimerCounting += Time.deltaTime / AttackTime;
            }
            else
            {
                OnAttacking = false;
            }
        }
    }

    public void Fire()
    {
        Projectile.GetComponent<EnemyProjectileController>().isMovingLeft = Controller.IsWalkingLeft;
        Projectile.GetComponent<EnemyProjectileController>().AttackDamage = AttackDamage;
        Projectile.GetComponent<EnemyProjectileController>().AttackPushForce = AttackPushForce;

        GameObject NewProjectile =
            Instantiate(Projectile, transform.position,
            Quaternion.identity);
    }
}
