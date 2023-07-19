using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject objClone = Instantiate(prefab);
        objClone.transform.position = position;
        _placedGameObjects.Add(objClone);
        return _placedGameObjects.Count - 1;
    }
}
