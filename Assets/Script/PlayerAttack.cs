using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Player;

    public bool IsAttacking = false;
    public bool CanAttack = true;

    //public float AttackRange = 10f;
    //TImer : Combo Counting; Attack Freezing;

    public float AttackDash = 50f;
    public float AttackFreeze = 1f;
    public float AttackDamage = 1f;
    public float AttackPushForce = 10f;
    public float ComboEndCooldownTime = 3f;
    public float ComboCountInterval = 0.5f;
    public float ComboIntervalCounting;
    public float AttackFreezeTimer;

    public float PunchFreeze = 0.4f;
    public float PunchDamage = 10f;
    public float PunchDash = 30f;
    public float PunchPushForce = 0.4f;

    //S0=front; S1 = foward; S2 = Up; S3 = down

    //Special


    //Front Special

    public float S1Freeze = 0.8f;
    public float S1Damage = 20f;
    public float S1Dash = 50f;
    public float S1PushForce = 0.4f;

    //Up Special

    //Down Special



    public int ComboCount = 0;
    public int MaximumComboCount = 4;

    public GameObject AttackHitBox;
    public GameObject PlayerTransform;


    // Start is called before the first frame update
    void Start()
    {
        AttackHitBox.SetActive(false);
        ComboIntervalCounting = 1;
        AttackFreezeTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanAttack == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DoPunch();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {

                }
                else if (Input.GetAxis("Horizontal") != 0)
                {
                    DoPush();
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {

                }
                else
                {
                }
            }
        }


        //==============
        //Combo & Attack Freeze
        //==============

        //Combo Counting
        if (ComboIntervalCounting < 1)
        {
            ComboIntervalCounting += Time.deltaTime / ComboCountInterval;
        }

        else
        {
            ComboCount = 0;
        }
        

        //MaxCooldown
        if (ComboCount == MaximumComboCount)

        {
            AttackFreeze = ComboEndCooldownTime;
            ComboCount = 0;
            //StartCoroutine("ComboCooldown", ComboCooldownTime);
        }

        //Attack Freezing

        if (IsAttacking == true)
        {
            AttackHitBox.SetActive(true);
        }
        else
        {
            AttackHitBox.SetActive(false);
        }

        if (AttackFreezeTimer < 1)
        {
            CanAttack = false;
            AttackFreezeTimer += Time.deltaTime / AttackFreeze;
        }
        else
        {
            IsAttacking = false;
            CanAttack = true;
            ComboIntervalCounting = 0;
        }

    }

    //==========
    //Punch & Kick
    //==========

    public void DoPunch()
    {
        AttackFreeze = PunchFreeze;
        AttackDamage = PunchDamage;
        AttackDash = PunchDash;
        AttackPushForce = PunchPushForce;
        Debug.Log("Player Attack Punch");
        Attack();
    }

    public void DoPush()
    {
        AttackFreeze = S1Freeze;
        AttackDash = S1Dash;
        AttackPushForce = S1PushForce;
        Debug.Log("Player Attack Kick");
        Attack();
        //StartCoroutine("SetAttackFreeze", AttackFreeze);

    }

    private void Attack()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * transform.localScale.x * AttackDash, ForceMode.Impulse);
        ComboCount++;
        ComboIntervalCounting = -99;
        IsAttacking = true;
        AttackFreezeTimer = 0;
    }

    //IEnumerator SetAttackFreeze(float delay)
    //{
    //    ComboIntervalCounting = -99;
    //    ComboCount++;
    //    CanAttack = false;
    //    AttackHitBox.SetActive(true);
    //    yield return new WaitForSeconds(delay);

    //    if (ComboCount < MaximumComboCount)
    //    {
    //        CanAttack = true;
    //    }
        
    //    IsAttacking = false;
    //    AttackHitBox.SetActive(false);
    //    ComboIntervalCounting = 0;

    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //Deal Damage to Enemy
            other.SendMessage("ReceiveDamage", AttackDamage);
            other.SendMessage("ReceiveForce", AttackPushForce);

            Debug.Log("Hit Enemy");
        }

    }
}
