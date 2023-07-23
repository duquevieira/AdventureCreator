using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> PlacedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject objClone = Instantiate(prefab);
        objClone.transform.position = position;
        objClone.transform.rotation = rotation;
        PlacedGameObjects.Add(objClone);
        return PlacedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (PlacedGameObjects.Count <= gameObjectIndex || PlacedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(PlacedGameObjects[gameObjectIndex]);
        PlacedGameObjects[gameObjectIndex] = null;
    }

    public void RemoveAllObjects()
    {
        foreach(var obj in PlacedGameObjects)
        {
            Destroy(obj);
        }
        PlacedGameObjects.Clear();
    }
}
