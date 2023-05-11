using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Tale {
    [SerializeField]
    public List<ObjectInfo> propDataList;
    [SerializeField]
    public PositionCoordinates playerPos;


    public Tale(GameObject root, StoryEngineScript story) {
        propDataList = new List<ObjectInfo>();
        foreach (GameObject mapObject in GameObject.FindGameObjectsWithTag("Map"))
            propDataList.Add(mapObject.GetProp());
        GameObject player = story.Player;
        playerPos = new PositionCoordinates(player.transform.position.x, player.transform.position.z);
    }
}