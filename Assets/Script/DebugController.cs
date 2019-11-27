using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Camera;

    public Vector3 SpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnLocation = new Vector3(Camera.transform.position.x - 6.5f, 0, 0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync("Demo");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //game
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject newMeleeEnemy = Instantiate(Enemy, SpawnLocation, Quaternion.identity);
            //game
        }
    }
}
