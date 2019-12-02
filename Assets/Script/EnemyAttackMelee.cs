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

    public bool IsEngaging;
    public float RandomMoveRange;

    public float MaxFreeze = 0.5f;
    public float MaxMove = 1f;
    public float TimeMove = 99;
    public bool OnFreeze = false;
    public bool OnMove = false;
    public float TimeFreeze = 0;
    public float MoveZChance = 0;
    public bool RandomMove = true;
    public float RandomRangeInterval = 1f;

    private float randnum = 0;


    // Start is called before the first frame update
    void Awake()
    {
        Target = GameObject.Find("Player");

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

        if (!IsEngaging && !Controller.OnStun)
        {
            OnChecking = false;
            OnAttacking = false;
            OnCharging = false;
            ChargeTimerCounting = 1f;
            AttackTimerCounting = 1f;

            if (RandomMove && MoveZChance < 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x + (RandomMoveRange * randnum),
                    transform.position.y,
                    transform.position.z - (RandomMoveRange)),
                    Controller.WalkSpeed * Time.deltaTime);
            }
            else if (RandomMove && MoveZChance >= 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x + (RandomMoveRange * randnum),
                    transform.position.y,
                    transform.position.z + (RandomMoveRange)),
                    Controller.WalkSpeed * Time.deltaTime);
            }
            else if (!RandomMove)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                   new Vector3(transform.position.x + (2 * RandomMoveRange * randnum),
                   transform.position.y,
                   transform.position.z + (RandomMoveRange)), 0);
            }

            if (TimeFreeze < MaxFreeze & !RandomMove)
            {
                TimeFreeze += Time.deltaTime;
            }
            if (TimeFreeze >= MaxFreeze & !RandomMove) //ActiveOnMove
            {
                RandomMoveRange = Random.Range(RandomMoveRange - RandomRangeInterval, RandomMoveRange + RandomRangeInterval);
                MoveZChance = Random.Range(0.0f, 1.0f);
                TimeMove = 0;
                //OnFreeze = false;
                RandomMove = true;
            }
            if (TimeMove < MaxMove && RandomMove)
            {
                //transform.localPosition = NormalizedDirection * Controller.WalkSpeed * Time.deltaTime;
                TimeMove += Time.deltaTime;
            }
            if (TimeMove >= MaxMove && RandomMove) //ActiveOnFreeze
            {
                randnum = Random.Range(-1.0f, 1.0f);
                TimeFreeze = 0;
                RandomMove = false;

            }

            //===============
            //Aggro
            //===============
            if (GameObject.Find("Player") && !Controller.IsOutside)
            {
                RequestAttack();
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

                if (Physics.Raycast(transform.position, attackdir
                    , DetectRange, playerLayer) && AttackDelayTimerCounting >= 1)
                {
                //RequestAttack();
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
        else if (IsEngaging && !Controller.OnStun)
        {
            OnChecking = true;
        }


        if (Controller.OnStun)
        {
            ChargeTimerCounting = -1;
            OnCharging = false;
            CancelAttack();
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
                CancelAttack();
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

    public void RequestAttack()
    {
        Target.SendMessage("GetAttackRequest", gameObject);
        Debug.Log("Attack Requested");

    }

    public void AllowtoAttack()
    {
        IsEngaging = true;
        Debug.Log("Attack Allowed");

    }

    public void CancelAttack()
    {
        IsEngaging = false;
        Target.SendMessage("CancelAttacker", gameObject);
    }

}
