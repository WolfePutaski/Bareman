using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHPBarController : MonoBehaviour
{

    public Image[] RollBlock;
    public Image[] HPBlock;


    public int PlayerHP;
    public int PlayerMaxHP;

    public int PlayerRoll;
    public int PlayerMaxRoll;

    public GameObject Player;
    public PlayerController PlayerControl;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player)
        {
            PlayerControl = Player.GetComponent<PlayerController>();
            PlayerMaxHP = PlayerControl.HP;
            PlayerHP = PlayerMaxHP;

            PlayerMaxRoll = PlayerControl.MaxRollCount;
            PlayerRoll = PlayerMaxRoll;
        }
    }
    // Update is called once per frame
    void Update()
    {

        //=============
        //HP
        //=============
        PlayerHP = PlayerControl.HP;

        for (int i = 0; i < HPBlock.Length; i++)
        {
            //for disappearing hearts
            if (i < PlayerHP)
            {
                HPBlock[i].enabled = true;
            }
            else
            {
                HPBlock[i].enabled = false;
            }
        }

        //=============
        //Roll
        //=============
        PlayerRoll = PlayerControl.RollCount;

        for (int i = 0; i < RollBlock.Length; i++)
        {
            //for disappearing hearts
            if (i < PlayerRoll)
            {
                RollBlock[i].enabled = true;
            }
            else
            {
                RollBlock[i].enabled = false;
            }
        }
    }


}
