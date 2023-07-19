using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _clickedTileText;
    [SerializeField] private TMP_Text _clickedObjectText;
    [SerializeField] private Button _addButton;
    //[SerializeField] private Button _delButton;
    [SerializeField] private PlacementSystem _placementSys;
    [SerializeField] private SwitchCreateMode createMode;
    private List<GameObject> _mainObjects;

    private void Start()
    {
        _clickedTileText.text = "Clicked Tile: ";
        _clickedObjectText.text = "Clicked Object: ";
        //_addButton.interactable = false;
        //_delButton.interactable = false;
    }

    private void Awake()
    {
        
        _mainObjects = _placementSys._mainObjects;
    }

    private void Update()
    {
        //if (_dropdown.options.Count > 0)
        //{
        //    _addButton.interactable = true;
        //}
        if (createMode.currentMode == SwitchCreateMode.CreateMode.MapMode)
        {
            ShowUI();
            /*if (!_clickedObjectText.text.Equals("Clicked Object: "))
            {
                _addButton.interactable = true;
            }*/
            if (Input.GetMouseButtonDown(1))
            {
                //_dropdown.onValueChanged.AddListener(index => OnDropDownChange(_mainObjects, index));
                Vector3 clickedTile = _placementSys.getClickedTile();
                Vector3Int roundedClickedTile = _placementSys.convertFloatPosToTile(clickedTile);
                _clickedTileText.text = "Clicked Tile: " + roundedClickedTile;
            }
        } else
        {
            ClearUI();
        }
        
    }

    private void ClearUI()
    {
        _clickedTileText.enabled = false;
        _clickedObjectText.enabled = false;
        //_addButton.enabled = false;
    }

    private void ShowUI()
    {
        _clickedTileText.enabled = true;
        _clickedObjectText.enabled = true;
        //_addButton.enabled = true;
    }

    public void UpdateSelectectObject(string selectedObject)
    {
        _clickedObjectText.text = "Clicked Object: " + selectedObject;
    }
}
