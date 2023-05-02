using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUI : MonoBehaviour
{
    [SerializeField] public TMP_Dropdown _dropdown;
    [SerializeField] private TMP_Text _clickedTileText;
    [SerializeField] private Button _addButton;
    [SerializeField] private PlacementSystem _placementSys;
    private List<GameObject> _mainObjects;

    private void Start()
    {
        _dropdown.ClearOptions();
        _clickedTileText.text = "Clicked Tile: ";
        _addButton.interactable = false;
    }

    private void Awake()
    {
        
        _mainObjects = _placementSys._mainObjects;
    }

    private void Update()
    {
        if (_dropdown.options.Count > 0)
        {
            _addButton.interactable = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_mainObjects.ConvertAll(gameObject => gameObject.name));
            //_dropdown.onValueChanged.AddListener(index => OnDropDownChange(_mainObjects, index));
            Vector3 clickedTile = _placementSys.getClickedTile();
            Debug.Log(clickedTile);
            Vector3Int roundedClickedTile = _placementSys.convertFloatPosToTile(clickedTile);
            _clickedTileText.text = "Clicked Tile: " + roundedClickedTile;
        }
    }
}
