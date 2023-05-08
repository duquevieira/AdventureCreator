using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateStoryScript : MonoBehaviour
{

    private static string PARENTHESIS = "(";

    [SerializeField]
    private Button _buttonNewStep;
    [SerializeField]
    private Button _buttonCreateStory;
    [SerializeField]
    private GameObject _prefabNewStep;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Canvas _nodeCanvas;

    private List<GameObject> _allSteps;

    [SerializeField]
    private StoryEngineScript _storyEngineScript;

    void Start()
    {
        _allSteps = new List<GameObject>();
        _buttonNewStep.onClick.AddListener(AddNewStoryStep);
        _buttonCreateStory.onClick.AddListener(SaveStoryState);
    }

    private void AddNewStoryStep()
    {
        GameObject newStep = Instantiate(_prefabNewStep, _nodeCanvas.transform);
        Node nodeScript = newStep.GetComponent<Node>();
        nodeScript.ID = _allSteps.Count.ToString();
        _allSteps.Add(newStep);
    }

    private void SaveStoryState()
    {
        Storyboard story = new Storyboard();
        foreach(GameObject step in _allSteps)
        {
            StoryboardStep storyboardStep = new StoryboardStep(int.Parse(step.GetComponent<Node>().ID), step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0]);
            story.addStep(storyboardStep);
        }
        int i = 0;
        foreach(GameObject step in _allSteps)
        {
            Node nodeScript = step.GetComponent<Node>();
            List<Node> previous = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._in);
            StoryboardStep storyboardStep = story.getStorySteps()[i];
            foreach (Node requirement in previous)
            {
                storyboardStep.addRequirement(new ItemGroup(requirement.ID, 1));
            }
            List<Node> next = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._out);
            storyboardStep.addAcquires(new ItemGroup(nodeScript.ID, next.Count));
            Destroy(step);
            i++;
        }
        _storyEngineScript.Storyboard = story;
        _canvas.gameObject.SetActive(false);
    }
}
