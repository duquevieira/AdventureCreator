using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class World {
    //The World class saves the world itself
    public List<ObjectInfo> PropDataList;

    public World(PlacementSystem placer) {
        PropDataList = new List<ObjectInfo>();
        foreach(GameObject go in placer._objectsInScene) {
            PropDataList.Add(go.GetProp());
        }
    }
}