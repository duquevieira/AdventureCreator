using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateStoryScript : MonoBehaviour
{

    [SerializeField]
    private Button _buttonNewStep;
    [SerializeField]
    private Button _buttonCreateStory;
    [SerializeField]
    private GameObject _prefabNewStep;
    [SerializeField]
    private Canvas _canvas;

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
        GameObject newStep = Instantiate(_prefabNewStep, _canvas.transform);
        newStep.transform.SetParent(_canvas.transform, false);
        _allSteps.Add(newStep);
    }

    private void SaveStoryState()
    {
        Storyboard story = new Storyboard();
        int id = 0;
        foreach(GameObject step in _allSteps)
        {
            InputField[] inputFields = step.GetComponentsInChildren<InputField>();
            StoryboardStep storyboardStep = new StoryboardStep(id++, inputFields[0].text, inputFields[1].text);
            storyboardStep.addRequirement(new ItemGroup(inputFields[2].text.Split(" ")[1], int.Parse(inputFields[2].text.Split(" ")[0])));
            storyboardStep.addAcquires(new ItemGroup(inputFields[3].text.Split(" ")[1], int.Parse(inputFields[3].text.Split(" ")[0])));
            story.addStep(storyboardStep);
            Destroy(step);
        }
        _storyEngineScript.Storyboard = story;
        _canvas.gameObject.SetActive(false);
    }
}
