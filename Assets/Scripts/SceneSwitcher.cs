using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private GameObject current;
    public GameObject MAIN;
    public GameObject LOAD;
    public GameObject LOADING;

    void Start()
    {
        //Comença no Main Menu
        current = MAIN;
    }

    public void mainMenu()
    {
        if(current != MAIN)
        {
            current.SetActive(false);
            current = MAIN;
            current.SetActive(true);
        }
    }

    public void loadMenu()
    {
        if (current != LOAD)
        {
            current.SetActive(false);
            current = LOAD;
            current.SetActive(true);
        }
    }

    public void loadingScreen()
    {
        if (current != LOADING)
        {
            current.SetActive(false);
            current = LOADING;
            current.SetActive(true);
        }
    }
}
