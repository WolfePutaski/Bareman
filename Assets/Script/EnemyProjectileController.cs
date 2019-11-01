using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float Speed = 3f;
    public bool isMovingLeft;
    public float LifeTime = 1f;

    public int AttackDamage = 1;
    public float AttackPushForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Expire", LifeTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingLeft)
        {
            transform.Translate(-Vector3.right * Time.deltaTime * Speed);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * Speed);
        }
    }

    IEnumerator Expire(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("ReceiveDamage", AttackDamage);
            other.SendMessage("ReceiveForce", AttackPushForce);
            Destroy(this.gameObject);

            Debug.Log("Taken Damage");
        }
    }
}