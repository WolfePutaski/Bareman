using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int Phase = 0;
    public int OnAttackType = 1; //1 = Melee; 2 = Ranged; 3 = Special
    public float MaxHP;
    public EnemyController SelfController;
    public EnemyAttackMelee MeleeController;

    public float RangeCooldown;
    public float AoeCooldown;

//Attack Explanation:
// Has 3 attacks. Ranged and Soecial has its own cooldown progress. When using special attack, the ranged recharge is freeze
    // Start is called before the first frame update
    void Start()
    {
        MaxHP = SelfController.HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (SelfController.HP < (MaxHP/2))
        {
            Phase = 2;
        }

        if (Phase == 1)
        {

        }

        if (Phase == 2)
        {

        }
    }
}
