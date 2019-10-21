using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync("Demo");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //game
        }
    }
}
