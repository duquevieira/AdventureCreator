using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickMenuSlot : MonoBehaviour, IPointerClickHandler
{

    private PlacementUI _placUI;
    private PlacementSystem _placSys;
    private PlacementSystemV2 _placSys2;
    private string _selectedObjectName;
    private GameObject _selectedObject;

    private void Start()
    {
        _placUI = GameObject.Find("Grid").GetComponent<PlacementUI>();
        _placSys = GameObject.Find("Grid").GetComponent<PlacementSystem>();
        _placSys2 = GameObject.Find("Level").GetComponent<PlacementSystemV2>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*GameObject prev_SelectedObject = _selectedObject;
        _selectedObject = gameObject.transform.GetChild(0).gameObject;
        if (prev_SelectedObject != _selectedObject)
        {
            _placSys.SetSelectedObject(_selectedObject);
            string full_selectedObject = gameObject.transform.GetChild(0).gameObject.name;
            _selectedObjectName = full_selectedObject.Split("(")[0];
            _placUI.UpdateSelectectObject(_selectedObjectName);
        } else
        {
            _selectedObject = null;
            _placSys.SetSelectedObject(_selectedObject);
            _selectedObjectName = "";
            _placUI.UpdateSelectectObject(_selectedObjectName);
        }*/
        _selectedObject = gameObject.transform.GetChild(0).gameObject;
        string name = _selectedObject.name.Split("(")[0];
        _placSys2.StartPlacement(name);
    }

    public string getSelectedObjectName()
    {
        return _selectedObjectName;
    }

    public GameObject getSelectedObject()
    {
        return _selectedObject;
    }

}
