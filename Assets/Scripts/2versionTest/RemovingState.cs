using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int _gameObjectIndex = -1;
    string Name;
    Grid Grid;
    PreviewSystem PreviewSystem;
    GridData FloorData;
    GridData FurnitureData;
    ObjectPlacer ObjectPlacer;

    public RemovingState(string name, Grid grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        Name = name;
        Grid = grid;
        PreviewSystem = previewSystem;
        FloorData = floorData;
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
        if(!FurnitureData.CanPlacedObjectAt(gridPos,Vector2Int.one))
        {
            selectedData = FurnitureData;
        } else if (!FloorData.CanPlacedObjectAt(gridPos, Vector2Int.one))
        {
            selectedData = FloorData;
        }
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
        PreviewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        return !(FurnitureData.CanPlacedObjectAt(gridPos, Vector2Int.one) && !FloorData.CanPlacedObjectAt(gridPos, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool validity = CheckIfSelectionIsValid(gridPos);
        PreviewSystem.UpdatePosition(Grid.CellToWorld(gridPos), validity);
    }
}
