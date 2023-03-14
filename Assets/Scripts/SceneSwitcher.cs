using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void loadMenu()
    {
        SceneManager.LoadScene(1);
    }
}
