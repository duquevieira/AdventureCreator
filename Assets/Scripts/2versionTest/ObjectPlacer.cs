using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject objClone = Instantiate(prefab);
        objClone.transform.position = position;
        objClone.transform.rotation = rotation;
        _placedGameObjects.Add(objClone);
        return _placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (_placedGameObjects.Count <= gameObjectIndex || _placedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(_placedGameObjects[gameObjectIndex]);
        _placedGameObjects[gameObjectIndex] = null;
    }
}
