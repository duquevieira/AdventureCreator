using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject ground;
    public Slider scaleSlider;
    private float scaleSliderNumber;

    private void Start()
    {
        scaleSlider.value = 1;

    }

    private void Update()
    {
        scaleSliderNumber = scaleSlider.value;
        Vector3 scale = new Vector3(scaleSliderNumber, scaleSliderNumber, scaleSliderNumber);
        ground.transform.localScale = scale;
    }

}
