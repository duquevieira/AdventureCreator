using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Tale {
    [SerializeField]
    public List<ObjectInfo> PropDataList;
    [SerializeField]
    public PositionCoordinates PlayerPosition;


    public Tale(GameObject root, StoryEngineScript story) {
        PropDataList = new List<ObjectInfo>();
        foreach (GameObject mapObject in GameObject.FindGameObjectsWithTag("Map"))
            PropDataList.Add(mapObject.GetProp());
        GameObject player = story.Player;
        PlayerPosition = new PositionCoordinates(player.transform.position.x, player.transform.position.z);
    }
}