using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public bool Yes;
    public bool No;
    public bool Ans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Yes = (Ans);
        No = (!Yes);
    }
}
