using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex = -1;
    string Name;
    Grid Grid;
    PreviewSystem PreviewSystem;
    ObjectsDataBase Database;
    GridData FloorData;
    GridData FurnitureData;
    ObjectPlacer ObjectPlacer;

    public PlacementState(string name, Grid grid, PreviewSystem previewSystem, ObjectsDataBase database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        Name = name;
        Grid = grid;
        PreviewSystem = previewSystem;
        Database = database;
        FloorData = floorData;
        FurnitureData = furnitureData;
        ObjectPlacer = objectPlacer;

        _selectedObjectIndex = database.objectsDatabase.FindIndex(data => data.Name == name);
        if (_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsDatabase[_selectedObjectIndex].Prefab, database.objectsDatabase[_selectedObjectIndex].Size);
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

        int index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, Grid.CellToWorld(gridPos));

        GridData selectedData = Database.objectsDatabase[_selectedObjectIndex].Types == ObjectData.ObjectTypes.Floor ? FloorData : FurnitureData;
        selectedData.AddObjectAt(gridPos, Database.objectsDatabase[_selectedObjectIndex].Size, Database.objectsDatabase[_selectedObjectIndex].Name, index);
        PreviewSystem.UpdatePosition(Grid.CellToWorld(gridPos), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    {
        GridData selectedData = Database.objectsDatabase[selectedObjectIndex].Types == ObjectData.ObjectTypes.Floor ? FloorData : FurnitureData;
        return selectedData.CanPlacedObjectAt(gridPos, Database.objectsDatabase[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, _selectedObjectIndex);

        PreviewSystem.UpdatePosition(Grid.CellToWorld(gridPos), placementValidity);
    }

}
