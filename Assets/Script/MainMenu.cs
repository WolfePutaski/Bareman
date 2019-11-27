using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PrototypeStage()
    {
        SceneManager.LoadScene("PrototypeStage");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
