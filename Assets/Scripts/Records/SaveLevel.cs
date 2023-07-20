using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : AbstractSave
{
    private const string SAVE_ID = "019d5349-668e-458f-a112-a49970266f07";
    public static string SaveId = null;
    private string _prefabPath = "AndreUI_Test/";
    public PlacementSystem PlacementSystem;
    [SerializeField]
    private Camera _screenshotCamera;

    void Start()
    {
        if (!string.IsNullOrEmpty(SaveId))
        {
            Load();
        }
        else
        {
            PlacementSystem.SpawnFloorAndWalls();
        }
    }

    public async void Save()
    {
        await SaveBackgroundAsync();
    }

    private async Task SaveBackgroundAsync()
    {
        await Task.Yield();

        World world = new World(PlacementSystem);
        Debug.Log("World Saved");
        Tale tale = new Tale(Story, world, _screenshotCamera);
        Debug.Log("Tale Created");
        var taleWrapped = new DataTaleWrapper(tale);
        Debug.Log("Tale Wrapped");
        string json = JsonUtility.ToJson(taleWrapped, true);
        Debug.Log("Json made");
        if (string.IsNullOrEmpty(SaveId))
        {
            SaveId = OverwriteSaveProcess(await PostNewSaveAsync(json, SAVE_ID));
        }
        else
        {
            SaveId = OverwriteSaveProcess(await PutSaveAsync(json, SAVE_ID, SaveId));
        }
        Debug.Log("Done");
    }

    public void Load()
    {
        if (string.IsNullOrEmpty(SaveId))
            return;

        PlacementSystem.DestroyAllObjects();
        string json = GetSave(SaveId);
        Tale tale = GetSaveProcess(json);
        Debug.Log(tale);
        Story.Player.transform.position = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        PlayerHandler.Target = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        Story.Storyboard = tale.Storyboard;
        List<GameObject> toUpdate = new List<GameObject>();
        foreach (ObjectInfo objectInfo in tale.TaleWorld.ObjectsInWorld)
        {
            GameObject toAdd = Resources.Load<GameObject>(_prefabPath + objectInfo.Name);
            Debug.Log("Object: " + objectInfo.Name);
            toAdd.transform.position = new Vector3(objectInfo.Position.getRow(), 0, objectInfo.Position.getColumn());

            switch (objectInfo.Rotation.GetDirection())
            {
                case Direction.North:
                    toAdd.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.East:
                    toAdd.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.South:
                    toAdd.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Direction.West:
                    toAdd.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }

            var cloneObj = Instantiate(toAdd, toAdd.transform.position, toAdd.transform.rotation);
            cloneObj.name = cloneObj.name.Split('(')[0];
            toUpdate.Add(cloneObj);
        }
        PlacementSystem._objectsInScene = toUpdate;
    }

    private string OverwriteSaveProcess(string json)
    {
        return JsonUtility.FromJson<PostSaveResponse>(json).id;
    }

    private Tale GetSaveProcess(string json)
    {
        return JsonUtility.FromJson<DataTaleWrapper>(json).data;
    }
}
