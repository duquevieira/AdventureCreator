using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private GameObject current;
    public GameObject MAIN;
    public GameObject LOAD;

    void Start()
    {
        //Começ a no Main Menu
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
}
