using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : AbstractSave
{
    private const string GAME_ID = "63576297-97a1-4b79-8de9-c728219745eb";
    private string _prefabPath = "AndreUI_test/";
    public static string SaveId;
    public static string GameId = null;

    void Start() {
        if(string.IsNullOrEmpty(GameId)) LoadLevel(true);
        else LoadLevel(false); 
    }

    public void SaveState()
    {
        Game game = new Game(SaveId, Story);
        var gameWrapped = new DataGameWrapper(game);
        string json = JsonUtility.ToJson(gameWrapped, true);
        if(string.IsNullOrEmpty(GameId)) {
            GameId = PostNewSave(json, GAME_ID);
        } else {
            GameId = PutSave(json, GAME_ID, GameId);
        }
    }

    public void LoadState() {
        if(string.IsNullOrEmpty(GameId)) return;
        Story.ClearStoryElements();
        Game game = GetGameProcess(GetSave(GameId));
        Story.Player.transform.position = new Vector3(game.Player.getRow(), 0, game.Player.getColumn());
        Story.Storyboard = game.Storyboard;
        Story.StoryItems = game.StoryItems;
        Story.InventoryItems = game.InventoryItems;
    }

    private void NewGame(Tale tale) {
        Story.ClearStoryElements();
        Story.Player.transform.position = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        Story.Storyboard = tale.Storyboard;
    }

    private void LoadLevel(bool isNew) {
        Tale tale = GetTaleProcess(GetSave(SaveId));
        foreach(ObjectInfo o in tale.TaleWorld.ObjectsInWorld) {
            GameObject toAdd = Resources.Load<GameObject>(_prefabPath + o.Name);
            toAdd.transform.position = new Vector3(o.Position.getRow(), 0, o.Position.getColumn());
            switch(o.Rotation.GetDirection()) {
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
            var cloneObj = Instantiate(toAdd, toAdd.transform.position, toAdd.transform.rotation);
            cloneObj.name = cloneObj.name.Split("(")[0];
        }
        if(isNew) NewGame(tale);
        else LoadState();
    }

    private string OverwriteSaveProcess(string json) {
        return JsonUtility.FromJson<PostSaveResponse>(json).id;
    }

    private Game GetGameProcess(string json) {
        return JsonUtility.FromJson<DataGameWrapper>(json).data;
    }

    private Tale GetTaleProcess(string json) {
        return JsonUtility.FromJson<DataTaleWrapper>(json).data;
    }
}
