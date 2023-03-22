using System.Collections;
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
        if (id == -1)
        {
            sceneID = -1;
            levelName = "Empty";
        }
        else
        {
            sceneID = id;
            //Não dá para obter o nome de uma cena unloaded por isso tenho que apanhar o path e tratar o nome
            string[] path = SceneUtility.GetScenePathByBuildIndex(id).Split("/");
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
    public List<Level> levels = new List<Level>();
    public Text buttonTopText, buttonMiddleText, buttonBottomText;
    public GameObject Manager;
    private LoadMenuScript loadScript;
    private int page;
    private int max;

    void Start()
    {
        populateLevels(levels);
        page = 0;
        max = levels.Count;
        loadScript = (LoadMenuScript) Manager.GetComponent(typeof(LoadMenuScript));
        updateText();
    }

    public void nextPage()
    {
        if (2 + (page+1) * 3 < max)
        {
            page++;
            updateText();
        }
    }

    public void prevPage()
    {
        if (page > 0)
        {
            page--;
            updateText();
        }
    }

    public void topLoad()
    {
        loadScript.loadScene(levels[page * 3].getID());
    }

    public void midLoad()
    {
        loadScript.loadScene(levels[1 + page * 3].getID());
    }

    public void botLoad()
    {
        loadScript.loadScene(levels[2 + page * 3].getID());
    }

    private void updateText()
    {
        buttonTopText.text = levels[0 + page * 3].getName();
        buttonMiddleText.text = levels[1 + page * 3].getName();
        buttonBottomText.text = levels[2 + page * 3].getName();
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
