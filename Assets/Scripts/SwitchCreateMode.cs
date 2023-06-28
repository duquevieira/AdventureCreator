using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCreateMode : MonoBehaviour
{
    [HideInInspector] public enum CreateMode {MapMode, StoryBoardMode, TestingMode};
    public CreateMode currentMode;
    [SerializeField] private TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        currentMode = CreateMode.MapMode;
        dropdown.onValueChanged.AddListener(delegate { DropDownValueChanged(dropdown); });
    }

    /*public void SwitchMode()
    {
        if (currentMode == CreateMode.MapMode)
        {
            currentMode = CreateMode.StoryBoardMode;
        } else
        {
            currentMode = CreateMode.MapMode;
        }
    }*/

    private void DropDownValueChanged(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                currentMode = CreateMode.MapMode;
                break;
            case 1:
                currentMode = CreateMode.StoryBoardMode;
                break;
            case 2:
                currentMode = CreateMode.TestingMode;
                break;
            default:
                currentMode = CreateMode.MapMode;
                break;
        }
        
    }
}
