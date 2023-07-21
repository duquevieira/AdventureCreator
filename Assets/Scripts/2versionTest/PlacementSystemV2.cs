using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystemV2 : MonoBehaviour
{
    [SerializeField] private MouseCoords _inputManager;
    [SerializeField] private Grid _grid;
    [SerializeField] private ObjectsDataBase _database;
    [SerializeField] public GameObject _gridVisualization;
    [SerializeField] private PreviewSystem _preview;
    [SerializeField] private ObjectPlacer _objectPlacer;
    [SerializeField] private Vector2Int _gridSize;
    IBuildingState buildingState;
    private GridData _floorData, _structureData, _furnitureData;
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        _floorData = new GridData();
        _structureData = new GridData();
        _furnitureData = new GridData();
        _gridVisualization.transform.localScale = Vector3.zero + new Vector3(_gridSize.x/10f, 1, _gridSize.y / 10f);
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePos = _inputManager.getMouseWorldPosition();
        Vector3Int gridPos = _grid.WorldToCell(mousePos);
        if (_lastDetectedPosition != gridPos)
        {
            buildingState.UpdateState(gridPos);
            _lastDetectedPosition = gridPos;
        }

    }

    public void StartPlacement(string name)
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
        buildingState = new PlacementState(name,_grid,_preview,_database,_floorData,_structureData,_furnitureData,_objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
        buildingState = new RemovingState(/*name,*/ _grid, _preview, _floorData,_structureData, _furnitureData, _objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = _inputManager.getMouseWorldPosition();
        Vector3Int gridPos = _grid.WorldToCell(mousePos);
        buildingState.OnAction(gridPos);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    //{
    //    GridData selectedData = _database.objectsDatabase[selectedObjectIndex].Types == ObjectData.ObjectTypes.Floor ? _floorData : _furnitureData;
    //    return selectedData.CanPlacedObjectAt(gridPos, _database.objectsDatabase[selectedObjectIndex].Size);
    //}

    public void StopPlacement()
    {
        if (buildingState == null)
            return;
        _gridVisualization.SetActive(false);
        buildingState.EndState();
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        _lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    public void RotateStructure()
    {
        if (buildingState != null)
            buildingState.Rotate();
    }
        public void DragStructure()
    {
        if (buildingState != null)
            buildingState.Drag();
    }
}