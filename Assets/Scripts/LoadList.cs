using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

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
        Debug.Log("Awake");
        _availableSaves = new List<string>();
        _playerSaves = new Dictionary<string, string>();
        _levelImages = new List<string>();
        Task.Run(async () => await Startup()).ConfigureAwait(false);
    }

    private async Task Startup()
    {
        await Task.Run(() =>
        {
            populateLevels();
            populatePlayerSaves();
            updateLevel();
        });
    }

    public void selectSave() {
        if(_availableSaves.Count > 0) {
            SelectedSave = _availableSaves[_index];
        }
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
    
    public async void updateLevel() {
        if(_levelImages.Count > 0) {
            SaveImage.LoadImageFromBase64(_levelImages[_index]);
            SaveText.GetComponent<TextMeshProUGUI>().text = _availableSaves[_index];
            Debug.Log(_availableSaves[_index]);
        }
        await Task.Yield();
        Debug.Log("Attemped Update");
    }

    private async void populateLevels()
    {
        Debug.Log("Starting Population of Levels");
        string responseText = "";
        using (var httpClient = new HttpClient()) 
        {
            var response = await httpClient.GetAsync(GET_ALL_SAVES_ENDPOINT);
            responseText = await response.Content.ReadAsStringAsync();
        }
        if(string.IsNullOrEmpty(responseText)) return;
        GetAllSaves responseObject = JsonUtility.FromJson<GetAllSaves>(responseText);
        foreach(Document d in responseObject.Documents) {
            if(d.documentSchemaId.Equals(WorldSaveSchemaID)) {
                _availableSaves.Add(d.id);
                _levelImages.Add(d.data.Screenshot);
            }
        }
        Debug.Log("Ending Population of Levels");
    }

    private async void populatePlayerSaves()
    {
        Debug.Log("Populating Player Saves");
        string responseText = "";
        using (var httpClient = new HttpClient()) 
        {
            var response = await httpClient.GetAsync(GET_ALL_SAVES_ENDPOINT);
            responseText = await response.Content.ReadAsStringAsync();
        }
        GetAllGames responseObject = JsonUtility.FromJson<GetAllGames>(responseText);
        foreach(DocumentGame d in responseObject.Documents) {
            if(!d.documentSchemaId.Equals(WorldSaveSchemaID)) _playerSaves.Add(d.data.LevelId, d.id);
        }
        Debug.Log("Ending Population of Player Saves");
    }
}
