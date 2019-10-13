using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool IsAttacking = false;
    public bool CanAttack = true;
    public float AttackRange = 10f;
    public float AttackDelay = 0.4f;
    public float AttackDamage = 10f;
    public GameObject AttackHitBox;
    public GameObject PlayerTransform;


    // Start is called before the first frame update
    void Start()
    {
        AttackHitBox.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && CanAttack)
        {

            DoPunch();
        }
    }

    private void DoPunch()
    {
        IsAttacking = true;
        print("Player Attack Punch");
        StartCoroutine("SetAttackDelay", AttackDelay);
    }

    IEnumerator SetAttackDelay(float delay)
    {
        CanAttack = false;
        AttackHitBox.SetActive(true);
        yield return new WaitForSeconds(delay);
        CanAttack = true;
        IsAttacking = false;
        AttackHitBox.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AttackHitBox && other.tag == "Enemy")
        {
            //Deal Damage to Enemy
            other.SendMessage("ReceiveDamage", AttackDamage);
            print("Hit Enemy");
        }

    }
}
