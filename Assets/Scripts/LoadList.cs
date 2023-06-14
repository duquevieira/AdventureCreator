using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class LoadList : MonoBehaviour
{
    private const string GET_ALL_SAVES_ENDPOINT = "https://envoy-gw.orangesmoke-c07594bb.westeurope.azurecontainerapps.io/8cddde22-bd7d-4af9-8a2c-ceac14a35eae/document-api/api/v1/documents";
    private const string EMPTY = "Empty";
    private static int TOP_OFFSET = 0;
    private static int MID_OFFSET = 1;
    private static int BOT_OFFSET = 2;
    private static int LEVELS_PER_PAGE = 3;

    [SerializeField]
    private LoadMenuScript _loadScript;
    [SerializeField]
    private TransitionScript _transitionScript;

    public List<Tale> levels;
    public Text buttonTopText, buttonMiddleText, buttonBottomText;
    public MoreMountains.Tools.MMTouchButton resumeButton;
    private int _page;
    private int _max;

    void Awake()
    {
        levels = new List<Tale>();
        populateLevels(levels);
        _page = 0;
        _max = levels.Count;
        resumeButton.ButtonPressed.RemoveAllListeners();
        resumeButton.ButtonPressed.AddListener(() => changeResume(1));
        updateText();
    }

    public void nextPage()
    {
        if (BOT_OFFSET + (_page+1) * LEVELS_PER_PAGE < _max)
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
        _transitionScript.transition(0);
        resumeButton.ButtonPressed.RemoveAllListeners();
        resumeButton.ButtonPressed.AddListener(() => changeResume(1));
    }

    public void midLoad()
    {
        _transitionScript.transition(1);
        resumeButton.ButtonPressed.RemoveAllListeners();
        resumeButton.ButtonPressed.AddListener(() => changeResume(2));
    }

    public void botLoad()
    {
        _loadScript.loadScene(1); //TODO: Replace with the id of the scene in the project manager (Can't do right now because it depends on other things)
    }

    public void quitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void changeResume(int i) {
        _loadScript.loadScene(i);
    }

    private void updateText()
    {
        var top = TOP_OFFSET + _page * LEVELS_PER_PAGE;
        var mid = MID_OFFSET + _page * LEVELS_PER_PAGE;
        var bot = BOT_OFFSET + _page * LEVELS_PER_PAGE;
        if(top < levels.Count)
            buttonTopText.text = levels[top].Name;
        else
            buttonTopText.text = EMPTY;
        if(mid < levels.Count)
            buttonMiddleText.text = levels[mid].Name;
        else
            buttonMiddleText.text = EMPTY;
        if(bot < levels.Count)
            buttonBottomText.text = levels[bot].Name;
        else
            buttonBottomText.text = EMPTY;
    }

    private void populateLevels(List<Tale> levels)
    {
        var request = WebRequest.Create(GET_ALL_SAVES_ENDPOINT);
        var response = request.GetResponse();
        var stream = response.GetResponseStream();
        var reader = new StreamReader(stream);
        string responseText = reader.ReadToEnd();
        Debug.Log("Response: " + responseText);
        GetAllSavesResponse responseObject = JsonUtility.FromJson<GetAllSavesResponse>(responseText);
        Debug.Log(responseObject._rid);
        Debug.Log(responseObject.Documents.ToString());
        Debug.Log(responseObject._count);
        //TODO: Code to populate levels to be added here
    }

    private class GetAllSavesResponse
    {
        public string _rid;
        public List<Object> Documents;
        public int _count;
    }
}
