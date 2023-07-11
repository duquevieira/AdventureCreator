using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Net;

public class LevelSaveLoad : MonoBehaviour
{
    private const string WORLD = "World.data";
    private const string TALE = "Tale.data";
    private string _endpointPostPath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/document-schema/63576297-97a1-4b79-8de9-c728219745eb/documents";
    private string _endpointDeletePath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/";
    private string _endpointGetPath = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/documents/";
    private string _prefabPath = "AndreUI_test/";
    public string SaveId = null;
    public StoryEngineScript Story;
    public PlacementSystem PlacementSystem;

    //Test Variables
    private string testDirectory = "C:/Users/danie/Desktop/SavesFolder/temp.txt";

   
    public void Save()
    {
        World world = new World(PlacementSystem);
        Tale tale = new Tale(Story, world);
        var taleWrapped = new DataWrapper(tale);
        string json = JsonUtility.ToJson(taleWrapped, true);
        if(string.IsNullOrEmpty(SaveId)) {
            SaveId = PostNewSave(json);
        } else {
            SaveId = PutSave(json);
        }
        File.WriteAllText(testDirectory, json);
    }

    public void Load()
    {
        if(string.IsNullOrEmpty(SaveId)) return;
        PlacementSystem.DestroyAllObjects();
        Tale tale = GetSave();
        Debug.Log(tale);
        Story.Player.transform.position = new Vector3(tale.Player.getRow(), 0, tale.Player.getColumn());
        Story.Storyboard = tale.Storyboard;
        List<GameObject> toUpdate = new List<GameObject>();
        foreach(ObjectInfo objectInfo in tale.TaleWorld.ObjectsInWorld) {
            GameObject toAdd = Resources.Load<GameObject>(_prefabPath + objectInfo.Name);
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
            var cloneObj = Instantiate(toAdd, toAdd.transform.position, toAdd.transform.rotation);
            cloneObj.name = cloneObj.name.Split("(")[0];
            toUpdate.Add(cloneObj);
        }
        PlacementSystem._objectsInScene = toUpdate;
    }

    private string PostNewSave(string json)
    {
        var request = WebRequest.Create(_endpointPostPath);
        request.Method = "POST";
        request.ContentType = "application/json";
        return TreatResponse(json, request);
    }

    private string PutSave(string json)
    {
        var requestDelete = WebRequest.Create(_endpointDeletePath+SaveId);
        requestDelete.Method = "DELETE";
        requestDelete.ContentType = "application/json";
        TreatDeleteResponse(json, requestDelete);
        var requestPost = WebRequest.Create(_endpointPostPath);
        requestPost.Method = "POST";
        requestPost.ContentType = "application/json";
        return TreatResponse(json, requestPost);
    }

    private Tale GetSave() {
        var requestGet = WebRequest.Create(_endpointGetPath+SaveId);
        requestGet.Method = "GET";
        return TreatGetResponse(requestGet);
    }

    private static Tale TreatGetResponse(WebRequest request) {
        try
        {
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseJson = reader.ReadToEnd();
            Debug.Log("Get Response: " + responseJson);
            return JsonUtility.FromJson<GetResponse>(responseJson).data;
        }
        catch (WebException e)
        {
            Debug.Log("Error: " + e.Message);
        }
        return null;
    }

    private static string TreatResponse(string json, WebRequest request)
    {
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
        try
        {
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseJson = reader.ReadToEnd();
            return JsonUtility.FromJson<PostResponse>(responseJson).id;
        }
        catch (WebException e)
        {
            Debug.Log("Error: " + e.Message);
        }
        return null;
    }

    private static void TreatDeleteResponse(string json, WebRequest request)
    {
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
        try
        {
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseJson = reader.ReadToEnd();
        }
        catch (WebException e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }

    private class PostResponse {
        public string id;
        PostResponse(string id) {
            this.id = id;
        }
    } 

    private class GetResponse {
        public Tale data;

        GetResponse(Tale data) {
            this.data = data;
        }
    }
}