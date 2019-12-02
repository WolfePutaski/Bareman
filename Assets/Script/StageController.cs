using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("EnemyTypes")]
    [SerializeField]
    public int MaxWaveNum;
    private int WaveNum;

    public GameObject[] Enemy;
    private int RandomEnemy;

    //public int MaxEnemyA;
    //private int EnemyALeft;

    //public GameObject EnemyB;
    //public int MaxEnemyB;
    //private int EnemyBLeft;

    //public GameObject EnemyC;
    //public int MaxEnemyC;
    //private int EnemyCLeft;

    //GameObject NextEnemy;

    [Header("Spawner")]
    [SerializeField]

    public GameObject Spawner;

    public float SpawnZPos;

    public GameObject[] OnScreenEnemy;
    public int MaxOnScreenEnemy;

    public bool CanSpawn;

    public float SpawnTimeInterval;
    private float SpawnTimeCounting;
    public float RandomSpawnTimeCountingMin;
    public float RandomSpawnTimeCountingMax;


    public float CanSpawnTimer;
    public float CanSpawnTimeCount;

    [Header("Misc")]
    public CameraController Camera;


    // Start is called before the first frame update
    void Start()
    {
        SpawnTimeCounting = SpawnTimeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        OnScreenEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (CanSpawn)
        {
            Camera.CanFollow = false;

            if (CanSpawnTimeCount > CanSpawnTimer)
            {
                CanSpawn = false;
            }
            else
            {
                CanSpawnTimeCount += Time.deltaTime;
            }
        }
        if (CanSpawn && OnScreenEnemy.Length < MaxOnScreenEnemy)
        {
            if (SpawnTimeCounting < SpawnTimeInterval)
            {
                SpawnTimeCounting += Time.deltaTime;
            }
            else
            {
                SpawnEnemy();
                SpawnTimeCounting = 0;
            }
        }
        if (!CanSpawn)
        {
            if (OnScreenEnemy.Length == 0)
                {
                Camera.CanFollow = true;


            }
        }




    }

    void SpawnEnemy()
    {
        if (CanSpawn)
        {
            float Randomer = (Random.Range(0, 2)) * 2 - 1;
            SpawnTimeInterval = Random.Range(RandomSpawnTimeCountingMin, RandomSpawnTimeCountingMax);
            Spawner.transform.localPosition = new Vector3(6 * Randomer,1, Random.Range(-SpawnZPos, SpawnZPos));
            RandomEnemy = (Random.Range(0, Enemy.Length));
            Instantiate(Enemy[RandomEnemy], Spawner.transform.position,
            Quaternion.identity);
            Debug.Log("Enemy Spawned Type " + RandomEnemy);
        }
      
    }

}
