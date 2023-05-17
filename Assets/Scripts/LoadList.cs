using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level
{
    private int sceneID;
    private string levelName;

    public Level(int id)
    {
        sceneID = id;

        if (sceneID == -1) levelName = "Empty";
        else
        {
            //Não dá para obter o nome de uma cena unloaded por isso tenho que apanhar o path e tratar o nome
            string[] path = SceneUtility.GetScenePathByBuildIndex(sceneID).Split("/");
            levelName = path[path.Length-1].Split(".")[0];
        }
    }

    public string getName()
    {
        return levelName;
    }

    public int getID()
    {
        return sceneID;
    }
}



public class LoadList : MonoBehaviour
{
    private static int TOP_OFFSET = 0;
    private static int MID_OFFSET = 1;
    private static int BOT_OFFSET = 2;
    private static int LEVELS_PER_PAGE = 3;

    [SerializeField]
    private LoadMenuScript _loadScript;
    [SerializeField]
    private TransitionScript _transitionScript;

    public List<Level> levels;
    public Text buttonTopText, buttonMiddleText, buttonBottomText;
    public MoreMountains.Tools.MMTouchButton resumeButton;
    private int _page;
    private int _max;

    void Awake()
    {
        levels = new List<Level>();
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
        _loadScript.loadScene(levels[BOT_OFFSET + _page * LEVELS_PER_PAGE].getID());
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
        buttonTopText.text = levels[TOP_OFFSET + _page * LEVELS_PER_PAGE].getName();
        buttonMiddleText.text = levels[MID_OFFSET + _page * LEVELS_PER_PAGE].getName();
        buttonBottomText.text = levels[BOT_OFFSET + _page * LEVELS_PER_PAGE].getName();
    }

    private void populateLevels(List<Level> levels)
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for(int i = 1; i < sceneCount; i++)
        {
            levels.Add(new Level(i));
        }
        for(int i = 0; i <= levels.Count%3; i++)
        {
            levels.Add(new Level(-1));
        }
    }
}
