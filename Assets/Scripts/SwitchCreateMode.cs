using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCreateMode : MonoBehaviour
{
    [HideInInspector] public enum CreateMode {MapMode, StoryBoardMode, TestingMode};
    public CreateMode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        currentMode = CreateMode.MapMode;
    }

    public void SwitchMode()
    {
        if (currentMode == CreateMode.MapMode)
        {
            currentMode = CreateMode.StoryBoardMode;
        } else
        {
            currentMode = CreateMode.MapMode;
        }
    }
}
