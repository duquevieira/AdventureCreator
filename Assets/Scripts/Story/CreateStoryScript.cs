using MeadowGames.UINodeConnect4;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SwitchCreateMode;
using Connection = MeadowGames.UINodeConnect4.Connection;

public class CreateStoryScript : MonoBehaviour
{

    private static string PARENTHESIS = "(";
    private static string IN = "In";
    private static string OUT = "Out";
    private static string ANIMATION = "Animation ";

    [SerializeField]
    private Button _buttonNewStep;
    [SerializeField]
    private GameObject _prefabNewStep;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _stepParent;
    [SerializeField]
    private SwitchCreateMode switchMode;
    [SerializeField] ObjectsDataBase _database;


    private List<GameObject> _allSteps;
    private bool _firstSwapUpdate;

    [SerializeField]
    private StoryEngineScript _storyEngineScript;
    [SerializeField]
    private PrefabMenuScript _prefabMenuScript;

    private static int UILAYER = 5;
    private int defaultAnimationPosition;

    void Start()
    {
        ClearUI();
        _allSteps = new List<GameObject>();
        _firstSwapUpdate = false;
        _buttonNewStep.onClick.AddListener(AddNewStoryStep);
        defaultAnimationPosition = getDatabaseAnimationIndex("10");
    }

    private void AddNewStoryStep()
    {
        GameObject newStep = Instantiate(_prefabNewStep, _stepParent.transform);
        newStep.transform.localPosition = Vector3.zero;
        Node nodeScript = newStep.GetComponent<Node>();
        nodeScript.ID = (_allSteps.Count).ToString();
        nodeScript.ports[0].ID = IN + nodeScript.ID;
        nodeScript.ports[1].ID = OUT + nodeScript.ID;

        var obj = _database.objectsDatabase[defaultAnimationPosition];
        GameObject defaultAnimation = obj.Prefab;
        GameObject instantiated = Instantiate(defaultAnimation, newStep.transform.GetChild(4), false);
        instantiated.transform.rotation = obj.MiniatureRotation;
        instantiated.transform.position += obj.MinaturePosition;
        instantiated.transform.localPosition = new Vector3(0, 0, -10);
        int scale = obj.MiniatureScale;
        instantiated.transform.localScale = new Vector3(scale, scale, scale);
        instantiated.layer = UILAYER;
        foreach (Transform child in instantiated.transform)
            child.gameObject.layer = UILAYER;

        _allSteps.Add(newStep);
    }

    private int getDatabaseAnimationIndex(string animationNumber)
    {
        int i = 0;
        foreach (var obj in _database.objectsDatabase)
        {
            GameObject prefab = obj.Prefab;
            string[] result = prefab.gameObject.name.Split(ANIMATION);
            if (result.Length > 1 && result[1].Equals(animationNumber))
            {
                return i;
            }
            i++;
        }
        return 0;
    }

    public void SaveStoryState()
    {
        List<StoryboardStep> story = new List<StoryboardStep>();
        foreach (GameObject step in _allSteps)
        {
            int stepID = int.Parse(step.GetComponent<Node>().ID);
            string colliderName = "";
            if (step.transform.GetChild(2).childCount > 0)
                colliderName = step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0];
            StoryboardStep storyboardStep = new StoryboardStep(stepID, colliderName, step.transform.localPosition);
            int animation = int.Parse(step.transform.GetChild(4).GetChild(0).name.Split(ANIMATION)[1].Split(PARENTHESIS)[0]);
            storyboardStep.addAnimation(animation);
            story.Add(storyboardStep);

        }
        List<Port> portList = new List<Port>();
        for (int i = 0; i < _allSteps.Count; i++)
        {
            GameObject step = _allSteps[i];
            Node nodeScript = step.GetComponent<Node>();
            StepToggleScript toggleScript = step.GetComponent<StepToggleScript>();
            StoryboardStep storyboardStep = story[i];
            foreach (Connection connection in nodeScript.ports[0].Connections)
            {
                string[] name = connection.port0.ID.Split(OUT);
                int pos = int.Parse(name[1].Split(PARENTHESIS)[0]);
                if (_allSteps[pos].GetComponent<StepToggleScript>().ItemMode)
                {
                    storyboardStep.addRequirement(new ItemGroup(_allSteps[pos].transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0], 1));
                }
                else
                {
                    storyboardStep.addRequirement(new ItemGroup(name[1].Split(PARENTHESIS)[0], 1));
                }
            }
            int itemAmount = 0;
            if (toggleScript.ItemMode)
            {
                itemAmount = nodeScript.ports[1].ConnectionsCount;
                if(itemAmount != 0)
                    storyboardStep.addAcquires(new ItemGroup(step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0], itemAmount));
                foreach (Connection connection in nodeScript.ports[1].Connections)
                {
                    storyboardStep.addItemDependentStep(int.Parse(connection.port1.ID.Split(IN)[1]));
                }
            }
            List<Node> next = nodeScript.GetNodesConnectedToPolarity(Port.PolarityType._out);
            storyboardStep.addAcquires(new ItemGroup(nodeScript.ID, next.Count - itemAmount));
            portList.AddRange(nodeScript.ports);
            Destroy(step);
        }
        _allSteps.Clear();
        foreach (Port port in portList)
        {
            port.RemoveAllConnections();
        }
        _storyEngineScript.ClearStoryElements();
        _storyEngineScript.Storyboard = story;
    }

    private void LoadStoryState()
    {

        _allSteps = new List<GameObject>();
        List<StoryboardStep> storySteps = _storyEngineScript.Storyboard;
        foreach (StoryboardStep step in storySteps)
        {
            GameObject stepPrefab = Instantiate(_prefabNewStep, _stepParent.transform);

            var objAnimation = _database.objectsDatabase[getDatabaseAnimationIndex(step.getAnimation().ToString())];
            GameObject objectAnimation = objAnimation.Prefab;
            GameObject instantiatedAnimation = Instantiate(objectAnimation, stepPrefab.transform.GetChild(4), false);
            instantiatedAnimation.transform.rotation = objAnimation.MiniatureRotation;
            instantiatedAnimation.transform.position += objAnimation.MinaturePosition;
            instantiatedAnimation.transform.localPosition = new Vector3(0, 0, -10);
            int scaleAnimation = objAnimation.MiniatureScale;
            instantiatedAnimation.transform.localScale = new Vector3(scaleAnimation, scaleAnimation, scaleAnimation);
            instantiatedAnimation.layer = UILAYER;
            foreach (Transform child in instantiatedAnimation.transform)
                child.gameObject.layer = UILAYER;

            List<float> coords = step.getStepCoordinates();
            stepPrefab.transform.localPosition = new Vector3(coords[0], coords[1], coords[2]);
            Node nodeScript = stepPrefab.GetComponent<Node>();
            nodeScript.ID = step.getId().ToString();
            nodeScript.ports[0].ID = IN + nodeScript.ID;
            nodeScript.ports[1].ID = OUT + nodeScript.ID;
            foreach (ItemGroup acquired in step.getAcquired())
            {
                if (!int.TryParse(acquired.getItemName(), out int number))
                {
                    stepPrefab.GetComponent<StepToggleScript>().ToggleStepItem();
                    break;
                }
            }
            foreach (var obj in _database.objectsDatabase)
            {
                GameObject prefab = obj.Prefab;
                if (prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(step.getColliderName()))
                {
                    GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(2), false);
                    instantiated.transform.rotation = obj.MiniatureRotation;
                    instantiated.transform.position += obj.MinaturePosition;
                    instantiated.transform.localPosition = new Vector3(0, 0, -10);
                    int scale = obj.MiniatureScale;
                    instantiated.transform.localScale = new Vector3(scale, scale, scale);
                    instantiated.layer = UILAYER;
                    foreach (Transform child in instantiated.transform)
                        child.gameObject.layer = UILAYER;
                    break;
                }
            }
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
                if (int.TryParse(itemGroup.getItemName(), out int result))
                {
                    Node connectedNode = _allSteps[result].GetComponent<Node>();
                    nodeScript.ports[0].ConnectTo(connectedNode.ports[1]);
                }
            }
            foreach (int itemDependentStep in storyboardStep.getItemDependentSteps())
            {
                Node connectedNode = _allSteps[itemDependentStep].GetComponent<Node>();
                nodeScript.ports[1].ConnectTo(connectedNode.ports[0]);
            }
        }
    }
    private void ClearUI()
    {
        _buttonNewStep.gameObject.SetActive(false);
        _canvas.gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        _buttonNewStep.gameObject.SetActive(true);
        _canvas.gameObject.SetActive(true);
    }

    void Update()
    {
        if (switchMode.currentMode == SwitchCreateMode.CreateMode.StoryBoardMode)
        {
            ShowUI();
            if (!_firstSwapUpdate)
            {
                LoadStoryState();
                _firstSwapUpdate = true;
            }
        }
        else
        {
            if (_firstSwapUpdate)
            {
                SaveStoryState();
                _firstSwapUpdate = false;
            }
            ClearUI();
        }
        if (Input.GetKeyUp(KeyCode.F10))
        {
            SaveStoryState();
            _storyEngineScript.Storyboard = new List<StoryboardStep>();
            _allSteps = new List<GameObject>();
        }
    }
}