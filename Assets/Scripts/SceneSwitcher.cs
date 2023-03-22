using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private GameObject _current;
    public GameObject main;
    public GameObject load;

    void Start()
    {
        //Começ a no Main Menu
        _current = main;
    }

    public void mainMenu()
    {
        if(_current != main)
        {
            _current.SetActive(false);
            _current = main;
            _current.SetActive(true);
        }
    }

    public void loadMenu()
    {
        if (_current != load)
        {
            _current.SetActive(false);
            _current = load;
            _current.SetActive(true);
        }
    }
}
