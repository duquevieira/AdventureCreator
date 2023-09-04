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

    private GameObject _itemPort;
    private static String BUTTON_TEXT_STORY = "Story Step";
    private static String BUTTON_TEXT_ITEM = "Item Step";


    void Awake()
    {
        _buttonToggle.onClick.AddListener(ToggleStepItem);
        ItemMode = false;
        _itemPort = transform.GetChild(1).GetChild(2).gameObject;
    }

    public void ToggleStepItem()
    {
        Node nodeScript = GetComponent<Node>();
        if (ItemMode)
        {
            ItemMode = false;
            Port itemPort = nodeScript.ports[2];
            itemPort.RemoveAllConnections();
            _itemPort.SetActive(false);
            _buttonToggle.GetComponentInChildren<Text>().text = BUTTON_TEXT_STORY;
        }
        else
        {
            ItemMode = true;
            _itemPort.SetActive(true);
            nodeScript.ports[2].ID = "Item" + nodeScript.ID;
            _buttonToggle.GetComponentInChildren<Text>().text = BUTTON_TEXT_ITEM;
        }
    }
}
