using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScript : MonoBehaviour
{
    public GameObject officeLevel;
    public GameObject natureLevel;
    public Image fade;

    private GameObject _current;
    private GameObject _office;
    private GameObject _nature;
    private int currentIndex;

    void Start() 
    {
        _current = GameObject.Instantiate(officeLevel);
        _nature = GameObject.Instantiate(natureLevel);
        _office = _current;
        fade.enabled = false;
        currentIndex = 0;
    }

    public void transition(int i) {
        if(i == 0 && i != currentIndex) {
            StartCoroutine(fadeToLoad(_office));
            currentIndex = 0;
        }
        if(i == 1 && i != currentIndex) {
             StartCoroutine(fadeToLoad(_nature));
             currentIndex = 1;
        }
    }

    private IEnumerator fadeToLoad(GameObject next) {
        fade.enabled = true;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.5f)
        {
            Color c = fade.color;
            c.a = Mathf.Lerp(0.0f, 1.0f, t);
            fade.color = c;;
            yield return null;
        }

        _current.SetActive(false);

        _current = next;

        _current.SetActive(true);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.5f)
        {
            Color c = fade.color;
            c.a = Mathf.Lerp(1.0f, 0.0f, t);
            fade.color = c;
            yield return null;
        }

        fade.enabled = false;
    }

}
