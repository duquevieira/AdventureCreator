using MeadowGames.UINodeConnect4;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StepToggleScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Button _buttonToggle;

    [SerializeField]
    private Color _storyColour;
    [SerializeField]
    private Color _itemColour;

    [HideInInspector]
    public bool ItemMode;

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
            _buttonToggle.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.GetComponent<Image>().color = _storyColour;
        }
        else
        {
            ItemMode = true;
            _buttonToggle.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<Image>().color = _itemColour;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ItemMode)
            _buttonToggle.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ItemMode)
            _buttonToggle.transform.GetChild(1).gameObject.SetActive(false);
    }
}
