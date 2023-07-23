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
    GridData ObjData;
    ObjectPlacer ObjectPlacer;

    public RemovingState(/*string name,*/ Grid grid, PreviewSystem previewSystem, GridData objData, ObjectPlacer objectPlacer)
    {
        //Name = name;
        Grid = grid;
        PreviewSystem = previewSystem;
        ObjData = objData;
        ObjectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        PreviewSystem.StopShowingPreview();
    }


    public void OnAction(Vector3Int gridPos)
    {
        if (ObjData.CheckObjectsAt(gridPos))
        {
            _gameObjectIndex = ObjData.GetRepresenatationIndex(gridPos)[ObjData.GetRepresenatationIndex(gridPos).Count - 1];
            ObjData.RemoveTopObjectAt(gridPos);
            ObjectPlacer.RemoveObjectAt(_gameObjectIndex);
        }
        else
            return;
                      
        Vector3 cellPosition = Grid.CellToWorld(gridPos);
        PreviewSystem.UpdateCursorPosition(cellPosition, !CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        if (ObjData.CheckObjectsAt(gridPos))
            return true;
        else
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
