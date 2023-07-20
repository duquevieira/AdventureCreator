using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.IO;

public class LoadList : MonoBehaviour
{
    public string SelectedSave = null;
    public string UserSave = null;
    public ImageLoader SaveImage;
    public GameObject SaveText;
    private const string GET_ALL_SAVES_ENDPOINT = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/documents";
    private const string SAVE_ID = "019d5349-668e-458f-a112-a49970266f07";
    private const string EMPTY = "Empty";
    private const string WorldSaveSchemaID = "019d5349-668e-458f-a112-a49970266f07";
    private int _index = 0;
    private static List<string> _availableSaves;
    private static List<string> _levelImages;
    private static Dictionary<string, string> _playerSaves;

    void Awake()
    {
        _availableSaves = new List<string>();
        _playerSaves = new Dictionary<string, string>();
        _levelImages = new List<string>();
        populateLevels();
        populatePlayerSaves();
        updateLevel();
    }

    public void selectSave() {
        SelectedSave = _availableSaves[_index];
        if(_playerSaves.ContainsKey(SelectedSave)) {
            UserSave = _playerSaves[SelectedSave];
        }
    }

    public void prevSave() {
        Debug.Log("Previous!");
        if(_index >= 1) _index--;
        updateLevel();
    }

    public void nextSave() {
        Debug.Log("Next!");
        if(_index < _availableSaves.Count-1) _index++;
        updateLevel();
    }

    public void quitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    private void updateLevel() {
        if(_levelImages.Count > 0) {
            SaveImage.LoadImageFromBase64(_levelImages[_index]);
            SaveText.GetComponent<TextMeshProUGUI>().text = _availableSaves[_index];
            Debug.Log(_availableSaves[_index]);
        }
    }

    private void populateLevels()
    {
        var request = WebRequest.Create(GET_ALL_SAVES_ENDPOINT);
        var response = request.GetResponse();
        var stream = response.GetResponseStream();
        var reader = new StreamReader(stream);
        string responseText = reader.ReadToEnd();
        GetAllSaves responseObject = JsonUtility.FromJson<GetAllSaves>(responseText);
        foreach(Document d in responseObject.Documents) {
            if(d.documentSchemaId.Equals(WorldSaveSchemaID)) {
                _availableSaves.Add(d.id);
                _levelImages.Add(d.data.Screenshot);
            }
        }
    }

    private void populatePlayerSaves()
    {
        var request = WebRequest.Create(GET_ALL_SAVES_ENDPOINT);
        var response = request.GetResponse();
        var stream = response.GetResponseStream();
        var reader = new StreamReader(stream);
        string responseText = reader.ReadToEnd();
        GetAllGames responseObject = JsonUtility.FromJson<GetAllGames>(responseText);
        foreach(DocumentGame d in responseObject.Documents) {
            if(!d.documentSchemaId.Equals(WorldSaveSchemaID)) _playerSaves.Add(d.data.LevelId, d.id);
        }
    }
}
