using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class LevelSaveLoad : MonoBehaviour
{
    private const string WORLD = "World.data";
    private const string TALE = "Tale.data";
    private string FilePath = "C:/Users/linar/Documents/André/Tale.data";
    public StoryEngineScript Story;
    public PlacementSystem PlacementSystem;

    public void Save()
    {
        World world = new World(PlacementSystem);
        Tale tale = new Tale(Story, world);
        string json = JsonUtility.ToJson(tale, true);
        File.WriteAllText(FilePath, json);
    }

    public void Load()
    {
        string json = File.ReadAllText(FilePath);
        Tale tale = JsonUtility.FromJson<Tale>(json);
        Story.Player.transform.position = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        Story.Storyboard = tale.Storyboard;
        List<GameObject> toUpdate = new List<GameObject>();
        foreach(ObjectInfo objectInfo in tale.TaleWorld.ObjectsInWorld) {
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