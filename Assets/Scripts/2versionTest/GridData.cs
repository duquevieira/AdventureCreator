using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, List<PlacementData>> placedObjects = new Dictionary<Vector3Int, List<PlacementData>>();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, string name, int placedObjectIndex, ObjectData.ObjectTypes type)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        List<PlacementData> objectsAt;
        PlacementData data = new PlacementData(positionsToOccupy, name, placedObjectIndex, type);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                 objectsAt = placedObjects[pos];
                // throw new Exception($"Dictionary already contains this cell position {pos}");
            } else
            {
                objectsAt = new List<PlacementData>();
            }
            objectsAt.Add(data);
            placedObjects[pos] = objectsAt;
        }
    }

    public bool CanPlacedObjectAt(Vector3Int gridPosition, Vector2Int objectSize, ObjectData.ObjectTypes objType)
    {
        if (objType == ObjectData.ObjectTypes.Structure)
            return true;
        else
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                {
                    List<ObjectData.ObjectTypes> objectTypeAt = getObjectTypesAt(pos);
                    if (objectTypeAt.Contains(objType))
                        return false;
                }       
            }
            return true;
        }
    }

    public List<int> GetRepresentationIndex(Vector3Int gridPos)
    {
        List<int> objectsIndexes = new List<int>();
        if (!placedObjects.ContainsKey(gridPos))
            return null;
        else
        {
            List<PlacementData> objectsAt = placedObjects[gridPos];
            foreach(PlacementData obj in objectsAt)
            {
                objectsIndexes.Add(obj.PlacedObjectIndex);
            }
            return objectsIndexes;
        }
    }

    public void RemoveTopObjectAt(Vector3Int gridPos)
    {
        if (!CheckObjectsAt(gridPos))
            return;
        List<PlacementData> objectsAt = placedObjects[gridPos];
        PlacementData lastObject = objectsAt[objectsAt.Count-1];
        foreach(var pos in lastObject.occupiedPositions)
        {
            placedObjects[pos].Remove(lastObject);
            if (placedObjects[pos].Count == 0)
                placedObjects.Remove(pos);                
        }
    }

    public void RemoveAllObjects()
    {
        placedObjects.Clear();
    }

    public bool CheckObjectsAt(Vector3Int gridPos)
    {
        if (placedObjects.ContainsKey(gridPos))
            return true;
        else
            return false;
    }

    public PlacementData GetLastObjectAt(Vector3Int gridPos)
    {
        List<PlacementData> dataAt = placedObjects[gridPos];
        return dataAt[dataAt.Count-1];
    }

    public List<PlacementData> GetAllData()
    {
        List<PlacementData> allData = new List<PlacementData>();
        foreach(var pos in placedObjects.Keys)
        {
            foreach(var data in placedObjects[pos])
            {
                allData.Add(data);
            }
        }
        return allData;
    }

    public string getHighestObjectNameAt(Vector3Int gridPos)
    {
        List<PlacementData> objectsAt = placedObjects[gridPos];
        PlacementData lastObject = objectsAt[objectsAt.Count - 1];
        return lastObject.Name.Split("(")[0];   
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

    private List<ObjectData.ObjectTypes> getObjectTypesAt(Vector3Int pos)
    {
        List<ObjectData.ObjectTypes> objectTypesAt = new List<ObjectData.ObjectTypes>();
        foreach (var obj in placedObjects[pos])
        {
            if (!objectTypesAt.Contains(obj.Type))
                objectTypesAt.Add(obj.Type);
        }
        return objectTypesAt;
    }


    public class PlacementData
    {
        public List<Vector3Int> occupiedPositions;
        public string Name { get; private set; }
        public int PlacedObjectIndex { get; set; }

        public ObjectData.ObjectTypes Type { get; private set; }

        public PlacementData(List<Vector3Int> occupiedPositions, string name, int placedObjectIndex, ObjectData.ObjectTypes type)
        {
            this.occupiedPositions = occupiedPositions;
            Name = name;
            PlacedObjectIndex = placedObjectIndex;
            Type = type;

        }
    }
}
