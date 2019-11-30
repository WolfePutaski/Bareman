using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Camera;
    public CameraController CamController;

    public bool DebugEnabled = false;
    public Vector3 SpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            DebugEnabled = !DebugEnabled;
        }

        SpawnLocation = new Vector3(Camera.transform.position.x - 6.5f, Enemy.transform.localPosition.y, 0);
        if (DebugEnabled)
        {
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                CamController.CanFollow = !CamController.CanFollow;

            }
        }
    }
}
