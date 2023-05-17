using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelSaveLoad : MonoBehaviour
{
    public string FilePath = "C:/Users/danie/Desktop/SavesFolder/Level.data";
    public StoryEngineScript Story;
    public GameObject LevelRoot;

    public void Save()
    {
        string save = "";
        Tale tale = new Tale(LevelRoot, Story);
        PositionCoordinates playerPos = tale.PlayerPosition;
        string json = "Player:\n" + JsonUtility.ToJson(playerPos);
        save += json + "\n";
        save += "Objects:\n[\n";
        List<ObjectInfo> props = tale.PropDataList;
        foreach(ObjectInfo objectInf in props) {
            json = JsonUtility.ToJson(objectInf);
            save += json + "\n";
        }
        save += "]";
        File.WriteAllText(FilePath, save);
    }

    public void Load()
    {
        string json = File.ReadAllText(FilePath);
        string[] parts = json.Split("\n");
        PositionCoordinates objectData = JsonUtility.FromJson<PositionCoordinates>(parts[1]);
        Story.Player.transform.position = new Vector3(objectData.getRow(), 0, objectData.getColumn());
        for(int i = 4; i < parts.Length-1; i++)
        {
            ObjectInfo data = JsonUtility.FromJson<ObjectInfo>(parts[i]);
            if(data != null) {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Catalog/" + data.Name.Split(" ")[0] + ".prefab");
                GameObject obj = Instantiate(prefab, new Vector3(data.Position.getRow()+12.5f, 0, data.Position.getColumn()), Quaternion.identity, LevelRoot.transform);
                if(obj != null) {
                    obj.name = data.Name;
                    switch(data.Rotation.GetDirection()) {
                        case Direction.North:
                            obj.transform.rotation = Quaternion.Euler(0,0,0);
                            break;
                        case Direction.East:
                            obj.transform.rotation = Quaternion.Euler(0,90,0);
                            break;
                        case Direction.South:
                            obj.transform.rotation = Quaternion.Euler(0,180,0);
                            break;
                        case Direction.West:
                            obj.transform.rotation = Quaternion.Euler(0,270,0);
                            break;
                    }
                }
            }
        }
    }
}