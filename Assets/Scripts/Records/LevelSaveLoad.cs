using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelSaveLoad : MonoBehaviour
{
    public string filePath = "C:/Users/danie/Desktop/SavesFolder/Level.data";
    public StoryEngineScript story;
    public GameObject levelRoot;

    public void Save()
    {
        string save = "";
        Tale tale = new Tale(levelRoot, story);
        PositionCoordinates playerPos = tale.playerPos;
        string json = "Player:\n" + JsonUtility.ToJson(playerPos);
        save += json + "\n";
        save += "Objects:\n[\n";
        List<ObjectInfo> props = tale.propDataList;
        foreach(ObjectInfo o in props) {
            json = JsonUtility.ToJson(o);
            save += json + "\n";
        }
        save += "]";
        File.WriteAllText(filePath, save);
    }

    public void Load()
    {
        string json = File.ReadAllText(filePath);
        string[] parts = json.Split("\n");
        PositionCoordinates objectData = JsonUtility.FromJson<PositionCoordinates>(parts[1]);
        story.Player.transform.position = new Vector3(objectData.getRow(), 0, objectData.getColumn());
        for(int i = 4; i < parts.Length-1; i++)
        {
            ObjectInfo data = JsonUtility.FromJson<ObjectInfo>(parts[i]);
            if(data != null) {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Catalog/" + data.name.Split(" ")[0] + ".prefab");
                GameObject obj = Instantiate(prefab, new Vector3(data.pos.getRow()+12.5f, 0, data.pos.getColumn()), Quaternion.identity, levelRoot.transform);
                if(obj != null) {
                    obj.name = data.name;
                    if(data.rot.GetDirection() == Direction.North) obj.transform.rotation = Quaternion.Euler(0,0,0);
                    else if(data.rot.GetDirection() == Direction.East) obj.transform.rotation = Quaternion.Euler(0,90,0);
                    else if(data.rot.GetDirection() == Direction.South) obj.transform.rotation = Quaternion.Euler(0,180,0);
                    else obj.transform.rotation = Quaternion.Euler(0,270,0);
                }
            }
        }
    }
}