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

    public bool ItemMode;

    private GameObject _itemSpot;
    private GameObject _itemPort;
    private static string BUTTON_TEXT_STORY = "Story Step";
    private static string BUTTON_TEXT_ITEM = "Get Item";


    void Start()
    {
        _buttonToggle.onClick.AddListener(ToggleStepItem);
        ItemMode = false;
        _itemSpot = transform.GetChild(4).gameObject;
        _itemPort = transform.GetChild(1).GetChild(2).gameObject;
    }

    private void ToggleStepItem()
    {
        Node nodeScript = GetComponent<Node>();
        if (ItemMode)
        {
            ItemMode = false;
            Port itemPort = nodeScript.ports[2];
            itemPort.RemoveAllConnections();
            _itemSpot.SetActive(false);
            _itemPort.SetActive(false);
            _buttonToggle.GetComponentInChildren<Text>().text = BUTTON_TEXT_STORY;
        }
        else
        {
            ItemMode = true;
            _itemSpot.SetActive(true);
            _itemPort.SetActive(true);
            nodeScript.ports[2].ID = "Item" + nodeScript.ID;
            _buttonToggle.GetComponentInChildren<Text>().text = BUTTON_TEXT_ITEM;
        }
    }
}
