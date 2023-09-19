using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUI : MonoBehaviour
{
    //[SerializeField] private TMP_Text _clickedTileText;
    //[SerializeField] private TMP_Text _clickedObjectText;
    [SerializeField] private Button _addButton;
    //[SerializeField] private Button _delButton;
    [SerializeField] private PlacementSystem _placementSys;
    [SerializeField] private PlacementSystemV2 _placementSys2;
    [SerializeField] private SwitchCreateMode _createMode;
    [SerializeField] private TMP_Dropdown _environmentDropdown;
    [SerializeField] private TMP_Dropdown _objectTypesDropdown;
    [SerializeField] private ObjectsDataBase _database;
    private List<GameObject> _mainObjects;

    private void Start()
    {
        //_clickedTileText.text = "Clicked Tile: ";
        //_clickedObjectText.text = "Clicked Object: ";
        //_addButton.interactable = false;
        //_delButton.interactable = false;
    }

    private void Awake()
    {
        _mainObjects = _placementSys._mainObjects;
        string[] _environmentOptions = System.Enum.GetNames(typeof(ObjectData.ObjectEnvironemnts));
        string[] _objectTypesOptions = System.Enum.GetNames(typeof(ObjectData.ObjectTypes));
        _environmentDropdown.AddOptions(_environmentOptions.ToList());
        _objectTypesDropdown.AddOptions(_objectTypesOptions.ToList());

    }

    private void Update()
    {
        //if (_dropdown.options.Count > 0)
        //{
        //    _addButton.interactable = true;
        //}
        if (_createMode.currentMode == SwitchCreateMode.CreateMode.MapMode)
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
                //_clickedTileText.text = "Clicked Tile: " + roundedClickedTile;
            }
        } else
        {
            ClearUI();
        }
        
    }

    private void ClearUI()
    {
        //_clickedTileText.enabled = false;
        //_clickedObjectText.enabled = false;
        _placementSys2.StopPlacement();
             

        //_addButton.enabled = false;
    }

    private void ShowUI()
    {
        //_clickedTileText.enabled = true;
        //_clickedObjectText.enabled = true;
        _placementSys2._gridVisualization.SetActive(true);
        //_addButton.enabled = true;
    }

    public void UpdateSelectectObject(string selectedObject)
    {
        //_clickedObjectText.text = "Clicked Object: " + selectedObject;
    }
}
