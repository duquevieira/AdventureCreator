using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, string name, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, name, placedObjectIndex);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, y, 0));
            }
        }
        return returnVal;
    }

    public bool CanPlacedObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetRepresenatationIndex(Vector3Int gridPos)
    {
        if (!placedObjects.ContainsKey(gridPos))
            return -1;
        return placedObjects[gridPos].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPos)
    {
        foreach(var pos in placedObjects[gridPos].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }

    public class PlacementData
    {
        public List<Vector3Int> occupiedPositions;
        public string Name { get; private set; }
        public int PlacedObjectIndex { get; private set; }

        public PlacementData(List<Vector3Int> occupiedPositions, string name, int placedObjectIndex)
        {
            this.occupiedPositions = occupiedPositions;
            Name = name;
            PlacedObjectIndex = placedObjectIndex;
        }
    }
}
