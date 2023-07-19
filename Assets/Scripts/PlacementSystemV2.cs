using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystemV2 : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private MouseCoords inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDataBase database;
    [SerializeField] private GameObject gridVisualization;
    private int _selectedObjectIndex = -1;
    private GridData floorData, furnitureData;
    private Renderer previewRenderer;
    private List<GameObject> _placedGameObjects = new List<GameObject>();

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData= new GridData();
        previewRenderer= cellIndicator.GetComponentInChildren<Renderer>();
    }


    public void StartPlacement(string name)
    {
        StopPlacement();
        _selectedObjectIndex = database.objectsDatabase.FindIndex(data => data.Name == name);
        Debug.Log(_selectedObjectIndex);
        if (_selectedObjectIndex < 0) 
        {
            Debug.Log($"No name found {name}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.getMouseWorldPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        if (!placementValidity)
            return;
        GameObject objClone = Instantiate(database.objectsDatabase[_selectedObjectIndex].Prefab);
        objClone.transform.position = grid.CellToWorld(gridPos);
        _placedGameObjects.Add(objClone);
        GridData selectedData = database.objectsDatabase[_selectedObjectIndex].Types == ObjectData.ObjectTypes.Rug ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPos, database.objectsDatabase[_selectedObjectIndex].Size, database.objectsDatabase[_selectedObjectIndex].Name, _placedGameObjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsDatabase[selectedObjectIndex].Types == ObjectData.ObjectTypes.Rug ? floorData : furnitureData;
        return selectedData.CanPlacedObjectAt(gridPos, database.objectsDatabase[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        _selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (_selectedObjectIndex < 0)
            return;
        Vector3 mousePos = inputManager.getMouseWorldPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;
 
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
}
