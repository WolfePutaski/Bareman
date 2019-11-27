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
    public float AttackBlowType = 0;// 0= stagger, 1 = knock

    public float AttackFreezeTimer;

    [Header("Normal Attack")]
    public float PunchFreeze = 0.4f;
    public float PunchDamage = 10f;
    public float PunchDash = 30f;
    public float PunchPushForce = 0.4f;
    public int PunchBlowType = 0;// 0= stagger, 1 = knock


    //S0=still; S1 = foward; S2 = Up; S3 = down
    //Special
    [Header("Special Attack")]
    public float S0Freeze = 0.8f;
    public float S0Damage = 20f;
    public float S0Dash = 0;
    public float S0PushForce = 0;

    //Front Special
    [Header("Special Attack Front")]
    public float S1Freeze = 0.8f;
    public float S1Damage = 20f;
    public float S1Dash = 50f;
    public float S1PushForce = 0.4f;
    public int S1BlowType = 2;// 0= stagger, 1 = knock

    ////Up Special
    //[Header("Special Attack Up")]

    ////Down Special
    //[Header("Special Attack Down")]

    [Header("Tiring Setting")]
    public bool IsTired = false;
    public float TiringTimeCount;
    public float TiringTimeMax;

    [Header("Combo Counting")]
    public int ComboCount = 0;
    public int MaximumComboCount = 4;
    public float ComboEndCooldownTime = 3f;
    public float ComboCountInterval = 0.5f;
    public float ComboIntervalCounting;

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
                if (ComboCount == 3)
                {
                    DoLastPunch();
                }
                else
                {
                    DoPunch();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    DoUpperCut();
                }
                else if (Input.GetAxis("Horizontal") != 0)
                {
                    DoPush();
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {

                }
                else //No other key
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
            if (IsAttacking)
            {
                TiringTimeCount = 0;
            }
            IsAttacking = false;
            if (!IsTired)
            {
                CanAttack = true;
            }
            ComboIntervalCounting = 0;

        }

        if (IsTired)
        {
            if (TiringTimeCount < 1)
            {
                CanAttack = false;
                TiringTimeCount += Time.deltaTime / TiringTimeMax;
            }
            else if (TiringTimeCount >= 1)
            {
                IsTired = false;
                CanAttack = true;
            }
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
        AttackBlowType = PunchBlowType;
        Debug.Log("Player Attack Punch");
        Attack();
    }
    public void DoLastPunch()
    {
        AttackFreeze = PunchFreeze;
        AttackDamage = PunchDamage;
        AttackDash = PunchDash;
        AttackPushForce = PunchPushForce * 2f;
        AttackBlowType = 1;
        Debug.Log("Player Attack Last Punch");
        Attack();
    }

    public void DoPush()
    {
        AttackFreeze = S1Freeze;
        AttackDash = S1Dash;
        AttackPushForce = S1PushForce;
        AttackBlowType = S1BlowType;

        Debug.Log("Player Attack Kick");
        SpecialAttack();
        //StartCoroutine("SetAttackFreeze", AttackFreeze);

    }

    public void DoUpperCut()
    {

    }

    private void Attack()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * transform.localScale.x * AttackDash, ForceMode.VelocityChange);
        ComboCount++;
        ComboIntervalCounting = -99;
        IsAttacking = true;
        AttackFreezeTimer = 0;
    }

    private void SpecialAttack()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * transform.localScale.x * AttackDash, ForceMode.Impulse);
        IsAttacking = true;
        AttackFreezeTimer = 0;
        TiringTimeCount = -99;
        IsTired = true;
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
            other.SendMessage("ReceiveBlow", AttackBlowType);

            Debug.Log("Hit Enemy");
        }

    }
}
