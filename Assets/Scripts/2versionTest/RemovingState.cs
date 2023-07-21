using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RemovingState : IBuildingState
{
    private int _gameObjectIndex = -1;
    //string Name;
    Grid Grid;
    PreviewSystem PreviewSystem;
    GridData FloorData;
    GridData StructureData;
    GridData FurnitureData;
    ObjectPlacer ObjectPlacer;

    public RemovingState(/*string name,*/ Grid grid, PreviewSystem previewSystem, GridData floorData,GridData structureData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        //Name = name;
        Grid = grid;
        PreviewSystem = previewSystem;
        FloorData = floorData;
        StructureData = structureData;
        FurnitureData = furnitureData;
        ObjectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        PreviewSystem.StopShowingPreview();
    }


    public void OnAction(Vector3Int gridPos)
    {
        GridData selectedData = null;
        if (!FurnitureData.CanPlacedObjectAt(gridPos, Vector2Int.one))
        {
            selectedData = FurnitureData;
        }
        else if (!StructureData.CanPlacedObjectAt(gridPos, Vector2Int.one))
        {
            selectedData = StructureData;
        }
        else if (!FloorData.CanPlacedObjectAt(gridPos, Vector2Int.one))
            selectedData = FloorData;

        if (selectedData == null)
        {
            // nothing to remove
        } else
        {
            _gameObjectIndex = selectedData.GetRepresenatationIndex(gridPos);
            if (_gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPos);
            ObjectPlacer.RemoveObjectAt(_gameObjectIndex);
        }
        Vector3 cellPosition = Grid.CellToWorld(gridPos);
        PreviewSystem.UpdateCursorPosition(cellPosition, CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        if (!(FurnitureData.CanPlacedObjectAt(gridPos, Vector2Int.one)) || !(FloorData.CanPlacedObjectAt(gridPos, Vector2Int.one)))
            return true;
        return false;
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool validity = CheckIfSelectionIsValid(gridPos);
        PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), !validity);
    }

    public Quaternion Rotate()
    {
        return new Quaternion();
    }

    public void Drag()
    {
    }
}
