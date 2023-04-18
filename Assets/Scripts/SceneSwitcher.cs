using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private GameObject _current;

    [SerializeField]
    private GameObject _main;
    [SerializeField]
    private GameObject _load;

    void Start()
    {
        //Começa no Main Menu
        _current = _main;
    }

    public void mainMenu()
    {
        if(_current != _main)
        {
            _current.SetActive(false);
            _current = _main;
            _current.SetActive(true);
        }
    }

    public void loadMenu()
    {
        if (_current != _load)
        {
            _current.SetActive(false);
            _current = _load;
            _current.SetActive(true);
        }
    }
}
