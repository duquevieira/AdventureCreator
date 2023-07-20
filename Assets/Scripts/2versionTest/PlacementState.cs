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
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        if (!placementValidity)
            return;
        int index;
        GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);
        if (selectedData == StructureData)
        {
            index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, Grid.CellToWorld(gridPos), _rotation);
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), false);
            PreviewSystem.UpdatePreviewPosition(Grid.CellToWorld(gridPos), false);
        } else
        {
            index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, Grid.GetCellCenterWorld(gridPos), _rotation);
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), false);
            PreviewSystem.UpdatePreviewPosition(Grid.GetCellCenterWorld(gridPos), false);
        }
        selectedData.AddObjectAt(gridPos,_size, Database.objectsDatabase[_selectedObjectIndex].Name, index);
    }

    public Quaternion Rotate()
    {
        _rotation = PreviewSystem.RotatePreview();
        Vector2Int _newSize = PreviewSystem.RotateCursor(_size);
        //Database.objectsDatabase[_selectedObjectIndex].Size = _newSize; 
        _size = _newSize;
        return _rotation;
    }


    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);
        GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);

        if (selectedData == StructureData)
        {
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
            PreviewSystem.UpdatePreviewPosition(Grid.CellToWorld(gridPos), placementValidity);
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
