using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class World {
    //The World class saves the world itself
    public List<ObjectInfo> ObjectsInWorld;

    public World(PlacementSystem placer) {
        ObjectsInWorld = new List<ObjectInfo>();
        foreach(GameObject go in placer._objectsInScene) {
            ObjectsInWorld.Add(go.GetProp());
        }
    }
}