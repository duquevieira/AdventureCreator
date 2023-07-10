using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class LoadList : MonoBehaviour
{
    public string SelectedSave = null;
    public string UserSave = null;
    private const string GET_ALL_SAVES_ENDPOINT = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/documents";
    private const string EMPTY = "Empty";
    private const string WorldSaveSchemaID = "63576297-97a1-4b79-8de9-c728219745eb";
    private const int TOP_OFFSET = 0;
    private const int MID_OFFSET = 1;
    private const int BOT_OFFSET = 2;
    private const int LEVELS_PER_PAGE = 3;
    private int _top;
    private int _mid;
    private int _bot;
    private static List<string> _availableSaves;

    [SerializeField]
    private LoadMenuScript _loadScript;
    [SerializeField]
    private TransitionScript _transitionScript;
    public Text buttonTopText, buttonMiddleText, buttonBottomText;
    public MoreMountains.Tools.MMTouchButton resumeButton;
    private int _page;

    void Awake()
    {
        _page = 0;
        _availableSaves = new List<string>();
        populateLevels();
    }

    public void nextPage()
    {
        if (BOT_OFFSET + (_page+1) * LEVELS_PER_PAGE < _availableSaves.Count)
        {
            _page++;
            updateText();
        }
    }

    public void prevPage()
    {
        if (_page > 0)
        {
            _page--;
            updateText();
        }
    }

    public void topLoad()
    {
        SelectedSave = (_availableSaves.Count > _top) ? _availableSaves[_top] : null;
    }

    public void midLoad()
    {
        SelectedSave = (_availableSaves.Count > _mid) ? _availableSaves[_mid] : null;
    }

    public void botLoad()
    {
        SelectedSave = (_availableSaves.Count > _bot) ? _availableSaves[_bot] : null;
    }

    public void quitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void updateText()
    {
        _top = TOP_OFFSET + _page * LEVELS_PER_PAGE;
        _mid = MID_OFFSET + _page * LEVELS_PER_PAGE;
        _bot = BOT_OFFSET + _page * LEVELS_PER_PAGE;
        if(_top < _availableSaves.Count) {
            buttonTopText.text = _availableSaves[_top];
        } else
            buttonTopText.text = EMPTY;
        if(_mid < _availableSaves.Count)
            buttonMiddleText.text = _availableSaves[_mid];
        else
            buttonMiddleText.text = EMPTY;
        if(_bot < _availableSaves.Count)
            buttonBottomText.text = _availableSaves[_bot];
        else
            buttonBottomText.text = EMPTY;
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
            if(d.documentSchemaId.Equals(WorldSaveSchemaID)) _availableSaves.Add(d.id);
        }
        updateText();
    }
}
