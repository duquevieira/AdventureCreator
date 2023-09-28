using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SaveLevel : AbstractSave
{
    private const string SAVE_ID = "019d5349-668e-458f-a112-a49970266f07";
    public static string SaveId = null;
    private string _prefabPath = "Prefabs/";
    public PlacementSystemV2 PlacementSystem;
    public ObjectPlacer ObjectPlacer;
    [SerializeField] private Camera _screenshotCamera;
    [SerializeField] private TMP_InputField _inputfield;
    private string _saveName = null;
    [SerializeField]
    private CreateStoryScript createStoryScript;
    [SerializeField]
    private SwitchCreateMode switchMode;
    private float saveTimer;

    void Start() {
        saveTimer = 0.0f;
        _inputfield.onValueChanged.AddListener(delegate { OnValueChange(); });
        if(!string.IsNullOrEmpty(SaveId)) {
            Load();
        } else PlacementSystem.PlaceFloorAutomatically();
    }

    public void OnValueChange()
    {
        _saveName = _inputfield.text;
    }

    public async void Save()
    {
        if(saveTimer + 5f < Time.realtimeSinceStartup) {
            saveTimer = Time.realtimeSinceStartup;
            if (switchMode.currentMode == SwitchCreateMode.CreateMode.StoryBoardMode)
                createStoryScript.SaveStoryState();
            CanQuit = false;
            await SaveBackgroundAsync();
            CanQuit = true;
        }
    }

    private async Task SaveBackgroundAsync()
    {
        await Task.Yield();
        string name = _saveName;
        if (!string.IsNullOrEmpty(name))
        {
            World world = new World(ObjectPlacer);
            Debug.Log("World Saved");
            Tale tale = new Tale(name, Story, world, _screenshotCamera);
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
        } else
        {
            Debug.Log("Need to put a save name");
        }
    }

    public void Load()
    {
        if(string.IsNullOrEmpty(SaveId) || !CanQuit) 
            return;
        PlacementSystem.RemoveAllObjects();
        Tale tale = GetSaveProcess(GetSave(SaveId));
        Debug.Log(tale);
        _saveName = tale.Name;
        _inputfield.text = _saveName;
        Story.Player.transform.position = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        Story.Storyboard = tale.Storyboard;
        int index = 0;
        foreach(ObjectInfo objectInfo in tale.TaleWorld.ObjectsInWorld) {
            GameObject toAdd = Resources.Load<GameObject>(_prefabPath + objectInfo.Name.Split("(")[0]);
            toAdd.transform.position = new Vector3(objectInfo.Position.getRow(), objectInfo.Position.getHeight(), objectInfo.Position.getColumn());
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
            PlacementSystem.RestoreLoadedData(toAdd, index);
            index++;
        }
    }

    private string OverwriteSaveProcess(string json) {
        return JsonUtility.FromJson<PostSaveResponse>(json).id;
    }
    
    private Tale GetSaveProcess(string json) {
        return JsonUtility.FromJson<DataTaleWrapper>(json).data;
    }

}
