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
    [SerializeField]
    private PrefabMenuScript _prefabMenuScript;

    private static int UILAYER = 5;

    void Start()
    {
        _allSteps = new List<GameObject>();
        _buttonNewStep.onClick.AddListener(AddNewStoryStep);
        _buttonCreateStory.onClick.AddListener(SaveStoryState);
    }

    private void AddNewStoryStep()
    {
        GameObject newStep = Instantiate(_prefabNewStep, _nodeCanvas.transform);
        newStep.transform.localPosition = Vector3.zero;
        Node nodeScript = newStep.GetComponent<Node>();
        int counter = 0;
        if(_allSteps.Count != 0)
        {
            counter = int.Parse(_allSteps[_allSteps.Count-1].GetComponent<Node>().ID) + 1;
        }
        nodeScript.ID = (counter).ToString();
        _allSteps.Add(newStep);
    }

    private void SaveStoryState()
    {
        Storyboard story = new Storyboard();
        List<GameObject> emptySteps = new List<GameObject>();
        foreach(GameObject step in _allSteps)
        {
            if(step.transform.GetChild(2).childCount > 0)
            {
                int stepID = int.Parse(step.GetComponent<Node>().ID);
                string colliderName = step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0];
                StoryboardStep storyboardStep = new StoryboardStep(stepID, colliderName, step.transform.localPosition);
                story.addStep(storyboardStep);
            }
            else
            {
                emptySteps.Add(step);
            }
        }
        foreach(GameObject step in emptySteps)
        {
            _allSteps.Remove(step);
            Destroy(step);
        }
        List<Port> portList = new List<Port>();
        foreach(GameObject step in _allSteps)
        {
            Node nodeScript = step.GetComponent<Node>();
            List<Node> previous = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._in);
            foreach(StoryboardStep storyboardStep in story.getStorySteps())
            {
                if(storyboardStep.getId().ToString().Equals(nodeScript.ID))
                {
                    foreach (Node requirement in previous)
                    {
                        storyboardStep.addRequirement(new ItemGroup(requirement.ID, 1));
                    }
                    List<Node> next = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._out);
                    storyboardStep.addAcquires(new ItemGroup(nodeScript.ID, next.Count));
                    portList.AddRange(nodeScript.ports);
                    Destroy(step);
                    break;
                }
            }
        }
        _allSteps.Clear();
        foreach(Port port in portList)
        {
            port.RemoveAllConnections();
        }
        _storyEngineScript.Storyboard = story;
        _canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && !_canvas.gameObject.activeSelf)
        {
            _canvas.gameObject.SetActive(true);
            _allSteps = new List<GameObject>();
            List<StoryboardStep> storySteps = _storyEngineScript.Storyboard.getStorySteps();
            foreach(StoryboardStep step in storySteps)
            {
                GameObject stepPrefab = Instantiate(_prefabNewStep, _nodeCanvas.transform);
                stepPrefab.transform.localPosition = step.getStepCoordinates();
                Node nodeScript = stepPrefab.GetComponent<Node>();
                nodeScript.ID = step.getId().ToString();
                //substituir
                foreach(GameObject prefab in _prefabMenuScript.AllPrefabs)
                {
                    if (prefab.gameObject.name.Equals(step.getColliderName()))
                    {
                        GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(2), false);
                        //TODO
                        instantiated.transform.localScale = new Vector3(25, 25, 25);
                        instantiated.transform.localPosition = Vector3.zero;
                        instantiated.layer = UILAYER;
                        foreach (Transform child in instantiated.transform)
                            child.gameObject.layer = UILAYER;
                        break;
                    }
                }
                /*foreach(List<GameObject> prefabList in _prefabMenuScript.AllPrefabs)
                {
                    foreach (GameObject prefab in prefabList)
                    {
                        if (prefab.gameObject.name.Equals(step.getColliderName()))
                        {
                            GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(2), false);
                            //TODO
                            instantiated.transform.localScale = new Vector3(25, 25, 25);
                            instantiated.transform.localPosition = Vector3.zero;
                            instantiated.layer = UILAYER;
                            foreach (Transform child in instantiated.transform)
                                child.gameObject.layer = UILAYER;
                            break;
                        }
                    }
                }*/
                _allSteps.Add(stepPrefab);
            }
            foreach(GameObject step in _allSteps)
            {
                Node nodeScript = step.GetComponent<Node>();
                foreach(StoryboardStep storyboardStep in storySteps)
                {
                    if(storyboardStep.getId().ToString().Equals(nodeScript.ID))
                    {
                        List<ItemGroup> requirements = storyboardStep.getRequirements();
                        foreach (ItemGroup itemGroup in requirements)
                        {
                            foreach(GameObject connectingStep in _allSteps)
                            {
                                Node connectingNode = connectingStep.GetComponent<Node>();
                                if(itemGroup.getItemName().Equals(connectingNode.ID))
                                {
                                    nodeScript.ports[0].ConnectTo(connectingNode.ports[1]);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
