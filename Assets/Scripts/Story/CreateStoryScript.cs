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
    [SerializeField] 
    private SwitchCreateMode switchMode;
    

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
        nodeScript.ID = (_allSteps.Count).ToString();
        nodeScript.ports[0].ID = "In" + nodeScript.ID;
        nodeScript.ports[1].ID = "Out" + nodeScript.ID;
        _allSteps.Add(newStep);
    }

    private void SaveStoryState()
    {
        List<StoryboardStep> story = new List<StoryboardStep>();
        foreach(GameObject step in _allSteps)
        {
            
            int stepID = int.Parse(step.GetComponent<Node>().ID);
            string colliderName = "";
            if(step.transform.GetChild(2).childCount > 0)
                colliderName = step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0];
            StoryboardStep storyboardStep = new StoryboardStep(stepID, colliderName, step.transform.localPosition);
            story.Add(storyboardStep);
            
        }
        List<Port> portList = new List<Port>();
        for (int i = 0; i < _allSteps.Count; i++)
        {
            GameObject step = _allSteps[i];
            Node nodeScript = step.GetComponent<Node>();
            StoryboardStep storyboardStep = story[i];
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
            if (nodeScript.ports.Count > 2)
            {
                itemAmount = nodeScript.ports[2].ConnectionsCount;
                storyboardStep.addAcquires(new ItemGroup(step.transform.GetChild(4).GetChild(0).name, itemAmount));
                foreach (Connection connection in nodeScript.ports[2].Connections)
                {
                    storyboardStep.addItemDependentStep(int.Parse(connection.port1.ID.Split("In")[1]));
                }
            }
            List<Node> next = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._out);
            storyboardStep.addAcquires(new ItemGroup(nodeScript.ID, next.Count - itemAmount));
            portList.AddRange(nodeScript.ports);
            Destroy(step);
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
            //TODO ADICIONAR NOVO COM ITENS
            if (Input.GetKeyUp(KeyCode.L))
            {
                _canvas.gameObject.SetActive(true);
                _allSteps = new List<GameObject>();
                List<StoryboardStep> storySteps = _storyEngineScript.Storyboard;
                foreach (StoryboardStep step in storySteps)
                {
                    GameObject stepPrefab = Instantiate(_prefabNewStep, _nodeCanvas.transform);
                    List<float> coords = step.getStepCoordinates();
                    stepPrefab.transform.localPosition = new Vector3(coords[0], coords[1], coords[2]);
                    Node nodeScript = stepPrefab.GetComponent<Node>();
                    nodeScript.ID = step.getId().ToString();
                    nodeScript.ports[0].ID = "In" + nodeScript.ID;
                    nodeScript.ports[1].ID = "Out" + nodeScript.ID;
                    string acquiredName = "";
                    foreach(ItemGroup acquired in step.getAcquired())
                    {
                        if(!int.TryParse(acquired.getItemName(), out int number))
                        {
                            stepPrefab.GetComponent<StepToggleScript>().ToggleStepItem();
                            acquiredName = acquired.getItemName();
                            break;
                        }
                    }
                    //substituir
                    bool foundAcquired = false;
                    bool foundCollider = false;
                    if (acquiredName.Equals(""))
                        foundAcquired = true;
                    foreach (GameObject prefab in _prefabMenuScript.AllPrefabs)
                    {
                        if (foundAcquired && foundCollider)
                            break;
                        if(prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(acquiredName))
                        {
                            foundAcquired = true;
                            GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(4), false);
                            //TODO
                            instantiated.transform.localScale = new Vector3(25, 25, 25);
                            instantiated.transform.localPosition = Vector3.zero;
                            instantiated.layer = UILAYER;
                            foreach (Transform child in instantiated.transform)
                                child.gameObject.layer = UILAYER;
                        }
                        if (prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(step.getColliderName()))
                        {
                            foundCollider = true;
                            GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(2), false);
                            //TODO
                            instantiated.transform.localScale = new Vector3(25, 25, 25);
                            instantiated.transform.localPosition = Vector3.zero;
                            instantiated.layer = UILAYER;
                            foreach (Transform child in instantiated.transform)
                                child.gameObject.layer = UILAYER;
                            
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
                for (int i = 0; i < _allSteps.Count; i++)
                {
                    GameObject step = _allSteps[i];
                    Node nodeScript = step.GetComponent<Node>();
                    StoryboardStep storyboardStep = storySteps[i];
                    List<ItemGroup> requirements = storyboardStep.getRequirements();
                    foreach (ItemGroup itemGroup in requirements)
                    {
                        if(int.TryParse(itemGroup.getItemName(), out int result))
                        {
                            Node connectedNode = _allSteps[result].GetComponent<Node>();
                            nodeScript.ports[0].ConnectTo(connectedNode.ports[1]);
                        }
                    }
                    foreach(int itemDependentStep in storyboardStep.getItemDependentSteps())
                    {
                        Node connectedNode = _allSteps[itemDependentStep].GetComponent<Node>();
                        nodeScript.ports[2].ConnectTo(connectedNode.ports[0]);
                    }
                }
            }
        } else
        {
            ClearUI();
        }
    }
}
