using MeadowGames.UINodeConnect4;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepToggleScript : MonoBehaviour
{
    [SerializeField]
    private Button _buttonToggle;

    [HideInInspector]
    public bool ItemMode;

    private static String BUTTON_TEXT_STORY = "Story Step";
    private static String BUTTON_TEXT_ITEM = "Item Step";


    void Awake()
    {
        _buttonToggle.onClick.AddListener(ToggleStepItem);
        ItemMode = false;
    }

    public void ToggleStepItem()
    {
        Node nodeScript = GetComponent<Node>();
        if (ItemMode)
        {
            ItemMode = false;
            _buttonToggle.GetComponentInChildren<Text>().text = BUTTON_TEXT_STORY;
        }
        else
        {
            ItemMode = true;
            _buttonToggle.GetComponentInChildren<Text>().text = BUTTON_TEXT_ITEM;
        }
    }
}
