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
    [SerializeField] private GameObject _floorPrefab;
    IBuildingState buildingState;
    private GridData _objData;
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        _objData= new GridData();
        _gridVisualization.transform.localScale = Vector3.zero + new Vector3(_gridSize.x/10f, 1, _gridSize.y / 10f);
        PlaceFloorAutomatically();
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
        buildingState = new PlacementState(name,_grid,_preview,_database,_objData,_objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
        buildingState = new RemovingState(/*name,*/ _grid, _preview, _objData, _objectPlacer);
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

    public void StopPlacement()
    {
        _gridVisualization.SetActive(false);
        if (buildingState == null)
            return;
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

    public void PlaceFloorAutomatically()
    {
        int _selectedObjectIndex = _database.objectsDatabase.FindIndex(data => data.Name == _floorPrefab.name);
        for (int x = -(_gridSize.x/2); x < _gridSize.x/2; x++)
        {
            for (int y = -(_gridSize.y/2); y<_gridSize.y/2; y++)
            {
                Vector3Int _gridPos = new Vector3Int(x, y, 0);
                Quaternion _rotation = _database.objectsDatabase[_selectedObjectIndex].Prefab.transform.rotation;
                int _index = _objectPlacer.PlaceObject(_database.objectsDatabase[_selectedObjectIndex].Prefab, _grid.GetCellCenterWorld(_gridPos), _rotation);   
                _objData.AddObjectAt(_gridPos, _database.objectsDatabase[_selectedObjectIndex].Size, _database.objectsDatabase[_selectedObjectIndex].Name,_index, _database.objectsDatabase[_selectedObjectIndex].Types);
            }
        }
    }

    public void RemoveAllObjects()
    {
        _objData.RemoveAllObjects();
        _objectPlacer.RemoveAllObjects();
    }
}
