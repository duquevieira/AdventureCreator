using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadList : MonoBehaviour
{
    public List<string> levels = new List<string>();
    public Text buttonTop, buttonMiddle, buttonBottom;
    private int page = 0;
    private int max = 0;

    void Start()
    {
        //Debug Info (Using strings due to lack of level data structure)
        levels.Add("level1");
        levels.Add("level2");
        levels.Add("level3");
        levels.Add("level4");
        levels.Add("level5");
        levels.Add("level6");
        max = levels.Count;
        buttonTop.text = levels[0 + page * 3];
        buttonMiddle.text = levels[1 + page * 3];
        buttonBottom.text = levels[2 + page * 3];
    }

    public void nextPage()
    {
        if (2 + (page+1) * 3 < max)
        {
            page++;
            buttonTop.text = levels[0 + page * 3];
            buttonMiddle.text = levels[1 + page * 3];
            buttonBottom.text = levels[2 + page * 3];
        }
    }

    public void prevPage()
    {
        if (page > 0)
        {
            page--;
            buttonTop.text = levels[0 + page * 3];
            buttonMiddle.text = levels[1 + page * 3];
            buttonBottom.text = levels[2 + page * 3];
        }
    }

    public string getLevel(int index)
    {
        return levels[index];
    }
}
