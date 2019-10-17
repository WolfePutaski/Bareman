using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //when stumble = No Follow, No Move, No Attack
    // Controller: Sense: Brain;

    public GameObject Self;

    public float HP = 10f;
    public float WalkSpeed = 300.5f;

    public Vector3 Direction;

    public float StunTimer; //For different stun type
    public float StunTimerMax;
    public float StunTimeCounting;

    public bool OnMove = false;
    public bool OnFollow = false;
    public bool OnStun;
    public bool IsWalkingLeft = false;

    //==============
    //Attack
    //==============
    public GameObject Target;


    // Start is called before the first frame update
    void Start()
    {
        StunTimeCounting = 1f;
    }

    // Update is called once per frame
    void Update()
    {

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
        // Follow Check
        //=========

        if (OnStun)
        {
            OnFollow = false;
        }
        else
        {
            OnFollow = true;
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

    }
     
    //=========
    // Health
    //=========

    public void ReceiveDamage(int Damage)//, float Timer)
    {
        HP -= Damage;
     //   StunTimerMax = Timer;
        Stun();
        if (HP <= 0)
        {
            //if (IsBoss)
            //{
            //    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsWon = true;
            //}
            Destroy(this.gameObject);
        }
    }

    //=========
    // On Stun
    //=========
    
    public void Stun()
    {
        StunTimeCounting = 0;
        
    }


}


