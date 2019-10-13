using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float HP = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
         
    public void ReceiveDamage(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            //if (IsBoss)
            //{
            //    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsWon = true;
            //}
            Destroy(this.gameObject);
        }
    }

}


