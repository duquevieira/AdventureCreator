using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelSaveLoad : MonoBehaviour
{
    private const string WORLD = "World.data";
    private const string TALE = "Tale.data";
    public string FilePath = "C:/Users/danie/Desktop/SavesFolder/Level";
    public StoryEngineScript Story;
    public PlacementSystem PlacementSystem;
    public void SaveWorld()
    {
        World world = new World(PlacementSystem);
        string json = JsonUtility.ToJson(world,true);
        File.WriteAllText(FilePath+ WORLD, json);
    }

    public void SaveTale()
    {
        Tale tale = new Tale(Story);
        string json = JsonUtility.ToJson(tale, true);
        File.WriteAllText(FilePath+ TALE, json);
    }

    public void LoadTale()
    {
        string json = File.ReadAllText(FilePath+TALE);
        Tale tale = JsonUtility.FromJson<Tale>(json);
        Story.Player.transform.position = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        Story.Storyboard = tale.Storyboard;
    }

    public void LoadWorld()
    {
        string json = File.ReadAllText(FilePath+WORLD);
        World world = JsonUtility.FromJson<World>(json);
        List<GameObject> toUpdate = new List<GameObject>();
        foreach(ObjectInfo objectInfo in world.PropDataList) {
            GameObject toAdd = Resources.Load<GameObject>(objectInfo.Name);
            toAdd.transform.position = new Vector3(objectInfo.Position.getRow(), 0, objectInfo.Position.getColumn());
            switch(objectInfo.Rotation.GetDirection()) {
                        case Direction.North:
                            toAdd.transform.rotation = Quaternion.Euler(0,0,0);
                            break;
                        case Direction.East:
                            toAdd.transform.rotation = Quaternion.Euler(0,90,0);
                            break;
                        case Direction.South:
                            toAdd.transform.rotation = Quaternion.Euler(0,180,0);
                            break;
                        case Direction.West:
                            toAdd.transform.rotation = Quaternion.Euler(0,270,0);
                            break;
            }
            toUpdate.Add(toAdd);
        }
        PlacementSystem._objectsInScene = toUpdate;
    }
}