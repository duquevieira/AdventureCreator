using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SwitchCreateMode;
using Connection = MeadowGames.UINodeConnect4.Connection;

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
    [SerializeField] SwitchCreateMode switchMode;
    

    private List<GameObject> _allSteps;

    [SerializeField]
    private StoryEngineScript _storyEngineScript;
    [SerializeField]
    private PrefabMenuScript _prefabMenuScript;

    private static int UILAYER = 5;

    void Start()
    {
        ClearUI();
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
        nodeScript.ports[0].ID = "In" + nodeScript.ID;
        nodeScript.ports[1].ID = "Out" + nodeScript.ID;
        _allSteps.Add(newStep);
    }

    private void SaveStoryState()
    {
        List<StoryboardStep> story = new List<StoryboardStep>();
        foreach(GameObject step in _allSteps)
        {
            if(step.transform.GetChild(2).childCount > 0)
            {
                int stepID = int.Parse(step.GetComponent<Node>().ID);
                string colliderName = step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0];
                StoryboardStep storyboardStep = new StoryboardStep(stepID, colliderName, step.transform.localPosition);
                story.Add(storyboardStep);
            }
        }
        List<Port> portList = new List<Port>();
        foreach(GameObject step in _allSteps)
        {
            Node nodeScript = step.GetComponent<Node>();
            foreach(StoryboardStep storyboardStep in story)
            {
                if(storyboardStep.getId().ToString().Equals(nodeScript.ID))
                {
                    foreach (Connection connection in nodeScript.ports[0].Connections)
                    {
                        string[] name = connection.port0.ID.Split("Out");
                        if (name.Length > 1)
                        {
                            storyboardStep.addRequirement(new ItemGroup(name[1], 1));
                        }
                        else
                        {
                            name = connection.port0.ID.Split("Item");
                            storyboardStep.addRequirement(new ItemGroup(_allSteps[int.Parse(name[1])].transform.GetChild(4).GetChild(0).name, 1));
                        }
                    }
                    int itemAmount = 0;
                    if(nodeScript.ports.Count > 2)
                    {
                        itemAmount = nodeScript.ports[2].ConnectionsCount;
                        storyboardStep.addAcquires(new ItemGroup(step.transform.GetChild(4).GetChild(0).name, itemAmount));
                    }
                    List<Node> next = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._out);
                    storyboardStep.addAcquires(new ItemGroup(nodeScript.ID, next.Count - itemAmount));
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
    }
    private void ClearUI()
    {
        _buttonCreateStory.gameObject.SetActive(false);
        _buttonNewStep.gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        _buttonCreateStory.gameObject.SetActive(true);
        _buttonNewStep.gameObject.SetActive(true);
    }

    void Update()
    {
        if (switchMode.currentMode == SwitchCreateMode.CreateMode.StoryBoardMode)
        {
            ShowUI();
        } else
        {
            ClearUI();
        }
        //TODO ADICIONAR NOVO COM ITENS
        if (Input.GetKeyUp(KeyCode.A) && !_canvas.gameObject.activeSelf)
        {
            _canvas.gameObject.SetActive(true);
            _allSteps = new List<GameObject>();
            List<StoryboardStep> storySteps = _storyEngineScript.Storyboard;
            foreach(StoryboardStep step in storySteps)
            {
                GameObject stepPrefab = Instantiate(_prefabNewStep, _nodeCanvas.transform);
                List<float> coords = step.getStepCoordinates();
                stepPrefab.transform.localPosition = new Vector3(coords[0], coords[1], coords[2]);
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
