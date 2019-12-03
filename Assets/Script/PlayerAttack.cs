using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Player;

    public PlayerController Controller;

    public bool IsAttacking = false;
    private bool CanAttack = true;

    //public float AttackRange = 10f;
    //TImer : Combo Counting; Attack Freezing;

    private float AttackDash = 50f;
    private float AttackFreeze = 1f;
    private float AttackDamage = 1f;
    private float AttackPushForce = 10f;
    private float AttackBlowType = 0;// 0= stagger, 1 = knock
    private bool isAoe;
    private bool isPushUp;
    private bool isSpin;

    public GameObject AttackHitBox;


    public float AttackFreezeTimer;

    

    [Header("Normal Attack")]
    public GameObject FrontHitbox;
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
    public int S0BlowType = 2;// 0= stagger, 1 = knock

    public float SpinCount;
    private float TimeSpinCount = 0;

    //Front Special
    [Header("Special Attack Front")]
    public float S1Freeze = 0.8f;
    public float S1Damage = 20f;
    public float S1Dash = 50f;
    public float S1PushForce = 0.4f;
    public int S1BlowType = 2;// 0= stagger, 1 = knock

    ////Up Special
    [Header("Special Attack Up")]
    public float S2Freeze = 0.8f;
    public float S2Damage = 20f;
    public float S2Dash = 50f;
    public float S2PushForce = 0.4f;
    public int S2BlowType;// 0= stagger, 1 = knock

    ////Down Special
    [Header("Special Attack Down")]
    public float S3Freeze = 0.8f;
    public float S3Damage = 20f;
    public float S3Dash = 50f;
    public float S3PushForce = 0.4f;
    public int S3BlowType;// 0= stagger, 1 = knock

    public GameObject SlamHitbox;
    public Vector3 MaxMagnitude;

    [Header("Tiring Setting")]
    public bool IsTired = false;
    public float TiringTimeCount;
    public float TiringTimeMax;

    [Header("Combo Counting")]
    public int ComboCount = 0;
    public int MaximumComboCount = 4;
    public float ComboEndCooldownTime = 3f;
    public float ComboCountInterval = 0.5f;
    private float ComboIntervalCounting;

    public GameObject PlayerTransform;


    // Start is called before the first frame update
    void Awake()
    {
        ComboIntervalCounting = 1;
        AttackFreezeTimer = 1;
        MaxMagnitude = SlamHitbox.transform.localScale;
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
                else if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    DoKick();
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    DoGroundSlam();
                }
                else //No other key
                {
                    DoSpin();
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
                if (AttackHitBox.transform.localPosition.x < 0)
                {
                    AttackHitBox.transform.localPosition = new Vector3(-AttackHitBox.transform.localPosition.x,
                        AttackHitBox.transform.localPosition.y,
                        AttackHitBox.transform.localPosition.z);
                }
                isPushUp = false;
                isAoe = false;
                isSpin = false;
                IsTired = false;
                CanAttack = true;
            }
        }

        //=========
        //GroundSlam
        //=========

        if (SlamHitbox.activeSelf)
        {
            if (SlamHitbox.transform.localScale.x < MaxMagnitude.x)
            {
                SlamHitbox.transform.localScale += SlamHitbox.transform.localScale * (Time.deltaTime / 0.05f);
            }
            else
            {
                AttackHitBox.SetActive(false);
            }
        }

        //=========
        //Spin
        //=========

        if (isSpin)
        {
            Controller.InvincibleTime = 0;

            if (IsAttacking)
            {

                if (TimeSpinCount < AttackFreeze/SpinCount)
                {
                    TimeSpinCount += Time.deltaTime;
                }
                else
                {
                    AttackHitBox.transform.localPosition = new Vector3(-AttackHitBox.transform.localPosition.x,
                        AttackHitBox.transform.localPosition.y, 
                        AttackHitBox.transform.localPosition.z);

                    TimeSpinCount = 0;
                }

            }

        }

    }

    //==========
    //Punch & Kick
    //==========

    public void DoPunch()
    {
        AttackHitBox = FrontHitbox;
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
        AttackHitBox = FrontHitbox;
        AttackFreeze = PunchFreeze;
        AttackDamage = PunchDamage;
        AttackDash = PunchDash;
        AttackPushForce = PunchPushForce * 2f;
        AttackBlowType = 1;
        Debug.Log("Player Attack Last Punch");
        Attack();
    }

    public void DoSpin()
    {
        TimeSpinCount = 0;
        isSpin = true;
        isAoe = true;

        AttackHitBox = FrontHitbox;
        AttackFreeze = S0Freeze;
        AttackDash = S0Dash;
        AttackPushForce = S0PushForce;
        AttackBlowType = S0BlowType;

        Debug.Log("Player Attack Spin");
        SpecialAttack();
    }

    public void DoKick()
    {
        AttackHitBox = FrontHitbox;
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
        isPushUp = true;


        AttackHitBox = FrontHitbox;
        AttackFreeze = S2Freeze;
        AttackDash = S2Dash;
        AttackPushForce = S2PushForce;
        AttackBlowType = S2BlowType;

        Debug.Log("Player Attack Uppercut");
        SpecialAttack();
    }

    public void DoGroundSlam()
    {
        SlamHitbox.transform.localScale = MaxMagnitude * 0.05f;

        isAoe = true;
        AttackHitBox = SlamHitbox;
        AttackFreeze = S3Freeze;
        AttackDash = S3Dash;
        AttackPushForce = S3PushForce;
        AttackBlowType = S3BlowType;


        Debug.Log("Player Attack Ground Slam");

        SpecialAttack();

    }


    private void Attack()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * transform.localScale.x * AttackDash, ForceMode.Impulse);
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

            if (isAoe)
            {
                if (other.transform.position.x < transform.position.x)
                {
                    other.SendMessage("ReceiveForce", -AttackPushForce * transform.localScale.x);
                }
                else
                {
                    other.SendMessage("ReceiveForce", AttackPushForce * transform.localScale.x);
                }
            }
            else
            {
                other.SendMessage("ReceiveForce", AttackPushForce);
            }
  
            other.SendMessage("ReceiveBlow", AttackBlowType);

            if (isPushUp)
            {
                other.SendMessage("ReceiveForceUp", AttackPushForce * 100);

            }



            Debug.Log("Hit Enemy");
        }

    }
}
