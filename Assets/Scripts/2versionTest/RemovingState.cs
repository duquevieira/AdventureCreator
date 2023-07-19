using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : MonoBehaviour
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
        this.name = name;
        this.Grid = grid;
        this.PreviewSystem = previewSystem;
        this.FloorData = floorData;
        this.FurnitureData = furnitureData;
        this.ObjectPlacer = objectPlacer;
    }
}
