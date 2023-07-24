using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCreateMode : MonoBehaviour
{
    [HideInInspector] public enum CreateMode {MapMode, StoryBoardMode, TestingMode};
    public CreateMode currentMode;
    public SimplePressAnimation[] Icons;
    private const int TOGGLE = 0;
    private const int TEST = 1;
    //[SerializeField] private TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        currentMode = CreateMode.MapMode;
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

    /*private void DropDownValueChanged(TMP_Dropdown change)
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
        
    }*/

    public void ToggleMode() {
        if(currentMode.Equals(CreateMode.MapMode)) {
            currentMode = CreateMode.StoryBoardMode;
            Icons[TOGGLE].PlayAnimation();
        } else {
            currentMode = CreateMode.MapMode;
            Icons[TOGGLE].PlayAnimation();
        }
    }

    public void Test() {
        currentMode = CreateMode.TestingMode;
        Icons[TEST].PlayAnimation();
    }
}
