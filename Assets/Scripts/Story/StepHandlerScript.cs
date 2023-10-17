using MeadowGames.UINodeConnect4;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class StepHandlerScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Button _buttonToggle;

    [SerializeField]
    private Color _storyColour;
    [SerializeField]
    private Color _itemColour;

    void Awake()
    {
        _buttonToggle.onClick.AddListener(DeleteStep);
    }

    public void ToggleStepItem()
    {
        gameObject.GetComponent<Image>().color = _itemColour;
    }

    public void DeleteStep()
    {
        Node nodeScript = GetComponent<Node>();
        foreach (Port port in nodeScript.ports)
        {
            port.RemoveAllConnections();
        }
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(3).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(3).gameObject.SetActive(false);
    }
}
