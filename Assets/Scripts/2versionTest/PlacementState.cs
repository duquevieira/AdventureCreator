using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex = -1;
    private Quaternion _rotation = new Quaternion();
    private Vector2Int _size;
    private Vector3 _structureRotatedPosition = new Vector3();
    string Name;
    Grid Grid;
    PreviewSystem PreviewSystem;
    ObjectsDataBase Database;
    GridData FloorData;
    GridData StructureData;
    GridData FurnitureData;
    ObjectPlacer ObjectPlacer;

    public PlacementState(string name, Grid grid, PreviewSystem previewSystem, ObjectsDataBase database, GridData floorData, GridData structureData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        Name = name;
        Grid = grid;
        PreviewSystem = previewSystem;
        Database = database;
        FloorData = floorData;
        StructureData = structureData;
        FurnitureData = furnitureData;
        ObjectPlacer = objectPlacer;

        _selectedObjectIndex = database.objectsDatabase.FindIndex(data => data.Name == name);
        _size = Database.objectsDatabase[_selectedObjectIndex].Size;
        _rotation = database.objectsDatabase[_selectedObjectIndex].Prefab.transform.rotation;
        if (_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsDatabase[_selectedObjectIndex].Prefab, _size);
        }
        else
            throw new System.Exception($"No object with name {name}");
    }

    public void EndState()
    {
        PreviewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        int index;
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        if (!placementValidity)
            return;

        GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);
        if (selectedData == StructureData)
        {
            index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, _structureRotatedPosition, _rotation);
            PreviewSystem.UpdateCursorPosition(_structureRotatedPosition, false);
            PreviewSystem.UpdatePreviewPosition(_structureRotatedPosition, false);
        } else
        {
            index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, Grid.GetCellCenterWorld(gridPos), _rotation);
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), false);
            PreviewSystem.UpdatePreviewPosition(Grid.GetCellCenterWorld(gridPos), false);
        }
        selectedData.AddObjectAt(gridPos,_size, Database.objectsDatabase[_selectedObjectIndex].Name, index);
    }

    private Vector3 GetPositionOfRotatedStructure(Quaternion rotation, Vector3 gridPos )
    {
        switch(rotation.eulerAngles.y)
        {
            case 90:
                return gridPos;
            case 180:
                return gridPos += new Vector3Int(0, 0, 1);
            case 270:
                return gridPos += new Vector3Int(1, 0, 1);
            case 0:
                return gridPos += new Vector3Int(1, 0, 0);
            default:
                return gridPos;
        }
    }

    public Quaternion Rotate()
    {
        GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);
        if (selectedData != StructureData)
        {
            _rotation = PreviewSystem.RotatePreviewCenter();
            Vector2Int _newSize = PreviewSystem.RotateCursor(_size);
            _size = _newSize;
        } else
        {
            Transform transform = PreviewSystem.RotatePreviewEdges();
            _rotation = transform.rotation;
            _structureRotatedPosition = transform.position;
            Vector2Int _newSize = PreviewSystem.RotateCursor(_size);
            _size = _newSize;
        }
        return _rotation;
    }


    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);
        _structureRotatedPosition = GetPositionOfRotatedStructure(_rotation, Grid.CellToWorld(gridPos));

        if (selectedData == StructureData)
        {
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
            PreviewSystem.UpdatePreviewPosition(_structureRotatedPosition, placementValidity);
        }
        else
        {
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
            PreviewSystem.UpdatePreviewPosition(Grid.GetCellCenterWorld(gridPos), placementValidity);
        }
    }

    private GridData GetSelectedData(ObjectData.ObjectTypes objectType)
    {
        switch (objectType)
        {
            case ObjectData.ObjectTypes.Floor:
                return FloorData;
            case ObjectData.ObjectTypes.Wall:
                return StructureData;
            default:
                return FurnitureData;
        }
    }
    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    {
        GridData selectedData = GetSelectedData(Database.objectsDatabase[selectedObjectIndex].Types);
        return selectedData.CanPlacedObjectAt(gridPos, _size);
    }

}
