using MeadowGames.UINodeConnect4;
using MeadowGames.UINodeConnect4.GraphicRenderer;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEditor;
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
    [SerializeField]
    private Transform characterSkinSpot;

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
        List<GameObject> allStepsClone = new List<GameObject>(_allSteps);
        foreach (GameObject step in allStepsClone)
        {
            if (!step.activeSelf)
            {
                _allSteps.Remove(step);
            }
        }
        for (int i = 0; i < _allSteps.Count; i++)
        {
            Node nodeScript = _allSteps[i].GetComponent<Node>();
            nodeScript.ID = (i).ToString();
            nodeScript.ports[0].ID = IN + nodeScript.ID;
            nodeScript.ports[1].ID = OUT + nodeScript.ID;
        }
        foreach (GameObject step in _allSteps)
        {
            int stepID = int.Parse(step.GetComponent<Node>().ID);
            string colliderName = "";
            if (step.transform.GetChild(2).childCount > 0)
                colliderName = step.transform.GetChild(2).GetChild(0).name.Split(PARENTHESIS)[0];
            StoryboardStep storyboardStep = new StoryboardStep(stepID, colliderName, step.transform.localPosition);
            int animation = int.Parse(step.transform.GetChild(4).GetChild(0).name.Split(ANIMATION)[1].Split(PARENTHESIS)[0]);
            storyboardStep.addAnimation(animation);
            TextMeshProUGUI dialogText = step.transform.GetChild(6).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            storyboardStep.addDialog(dialogText.text);
            int npcAnimation = 0;
            if(colliderName.Contains("Character_"))
            {
                Animator NPCAnimator = step.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>();
                npcAnimation = NPCAnimator.GetInteger("targetAnimation");
            }
            storyboardStep.addNPCAnimation(npcAnimation);
            story.Add(storyboardStep);

        }
        List<Port> portList = new List<Port>();
        for (int i = 0; i < _allSteps.Count; i++)
        {
            GameObject step = _allSteps[i];
            Node nodeScript = step.GetComponent<Node>();
            StepHandlerScript toggleScript = step.GetComponent<StepHandlerScript>();
            StoryboardStep storyboardStep = story[i];
            if (step.transform.GetChild(7).childCount > 0)
            {
                storyboardStep.addRequirement(new ItemGroup(step.transform.GetChild(7).GetChild(0).name.Split(PARENTHESIS)[0], 1));
            }
            if (step.transform.GetChild(5).childCount > 0)
            {
                storyboardStep.addAcquires(new ItemGroup(step.transform.GetChild(5).GetChild(0).name.Split(PARENTHESIS)[0], 1));
            }
            foreach (Connection connection in nodeScript.ports[0].Connections)
            {
                string[] name = connection.port0.ID.Split(OUT);
                string connectionName = name[1] + ":" + nodeScript.ID;
                if (connection.line.color == Color.red)
                    connectionName += "Mandatory";
                storyboardStep.addRequirement(new ItemGroup(connectionName, 1));
            }
            foreach (Connection connection in nodeScript.ports[1].Connections)
            {
                string[] name = connection.port1.ID.Split(IN);
                string connectionName = nodeScript.ID + ":" + name[1];
                if (connection.line.color == Color.red)
                    connectionName += "Mandatory";
                storyboardStep.addAcquires(new ItemGroup(connectionName, 1));
            }
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
        if (characterSkinSpot.childCount > 0)
        {
            _storyEngineScript.PlayerSkin = characterSkinSpot.GetChild(0).name.Split(PARENTHESIS)[0];
            Destroy(characterSkinSpot.GetChild(0).gameObject);
        }
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
            int scaleAnimation = objAnimation.MiniatureScale;
            instantiatedAnimation.transform.localScale = new Vector3(scaleAnimation, scaleAnimation, scaleAnimation);
            instantiatedAnimation.layer = UILAYER;
            foreach (Transform child in instantiatedAnimation.transform)
                child.gameObject.layer = UILAYER;
            instantiatedAnimation.transform.localPosition = new Vector3(0, 0, -10);

            List<float> coords = step.getStepCoordinates();
            stepPrefab.transform.localPosition = new Vector3(coords[0], coords[1], coords[2]);
            Node nodeScript = stepPrefab.GetComponent<Node>();
            nodeScript.ID = step.getId().ToString();
            nodeScript.ports[0].ID = IN + nodeScript.ID;
            nodeScript.ports[1].ID = OUT + nodeScript.ID;
            TMP_InputField dialogText = stepPrefab.transform.GetChild(6).GetComponent<TMP_InputField>();
            dialogText.text = step.getDialog();
            bool getItems = false;
            foreach (ItemGroup acquired in step.getAcquired())
            {
                string[] names = acquired.getItemName().Split(":");
                string[] mandatoryCheck = names;
                if (names.Length > 1)
                {
                    mandatoryCheck = names[1].Split("Mandatory");
                }
                if(acquired.getItemName().Contains("Finish"))
                {
                    Node connectedNode = _stepParent.transform.GetChild(2).GetComponent<Node>();
                    Connection connection = nodeScript.ports[1].ConnectTo(connectedNode.ports[0]);
                    if (acquired.getItemName().Contains("Mandatory"))
                        connection.line.color = Color.red;
                }
                else if (!int.TryParse(mandatoryCheck[0], out int number))
                {
                    stepPrefab.GetComponent<StepHandlerScript>().ToggleStepItem();
                    getItems = true;
                    break;
                }
            }
            bool hasRequirement = false;
            foreach (ItemGroup requirement in step.getRequirements())
            {
                string[] names = requirement.getItemName().Split(":");
                string[] mandatoryCheck = names;
                if (names.Length > 1)
                {
                    mandatoryCheck = names[0].Split("Mandatory");
                }
                if (names[0].Contains("Start"))
                {
                    Node connectedNode = _stepParent.transform.GetChild(1).GetComponent<Node>();
                    Connection connection = nodeScript.ports[0].ConnectTo(connectedNode.ports[0]);
                    if (requirement.getItemName().Contains("Mandatory"))
                        connection.line.color = Color.red;
                }
                else if (!int.TryParse(mandatoryCheck[0], out int number))
                {
                    hasRequirement = true;
                    break;
                }
            }
            bool foundCollider = false;
            bool foundCharacterSkin = false;
            foreach (var obj in _database.objectsDatabase)
            {
                GameObject prefab = obj.Prefab;
                if (!foundCharacterSkin && prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(_storyEngineScript.PlayerSkin))
                {
                    GameObject instantiated = Instantiate(prefab, characterSkinSpot, false);
                    instantiated.transform.rotation = obj.MiniatureRotation;
                    instantiated.transform.position += obj.MinaturePosition;
                    int scale = obj.MiniatureScale;
                    instantiated.transform.localScale = new Vector3(scale, scale, scale);
                    instantiated.layer = UILAYER;
                    foreach (Transform child in instantiated.transform)
                        child.gameObject.layer = UILAYER;
                    instantiated.transform.localPosition = new Vector3(0, 0, -10);
                    foundCharacterSkin = true;
                }
                if (!foundCollider && prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(step.getColliderName()))
                {
                    GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(2), false);
                    instantiated.transform.rotation = obj.MiniatureRotation;
                    instantiated.transform.position += obj.MinaturePosition;
                    int scale = obj.MiniatureScale;
                    instantiated.transform.localScale = new Vector3(scale, scale, scale);
                    instantiated.layer = UILAYER;
                    foreach (Transform child in instantiated.transform)
                        child.gameObject.layer = UILAYER;
                    instantiated.transform.localPosition = new Vector3(0, 0, -10);
                    foundCollider = true;

                    if (step.getColliderName().Contains("Character_"))
                    {
                        Animator NPCAnimator = instantiated.gameObject.GetComponent<Animator>();
                        NPCAnimator.SetInteger("targetAnimation", step.getNPCAnimation());
                    }
                }
                if (getItems && prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(step.getAcquired()[0].getItemName()))
                {
                    GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(5), false);
                    instantiated.transform.rotation = obj.MiniatureRotation;
                    instantiated.transform.position += obj.MinaturePosition;
                    int scale = obj.MiniatureScale;
                    instantiated.transform.localScale = new Vector3(scale, scale, scale);
                    instantiated.layer = UILAYER;
                    foreach (Transform child in instantiated.transform)
                        child.gameObject.layer = UILAYER;
                    instantiated.transform.localPosition = new Vector3(0, 0, -10);
                    getItems = false;
                }
                if (hasRequirement && prefab.gameObject.name.Split(PARENTHESIS)[0].Equals(step.getRequirements()[0].getItemName()))
                {
                    GameObject instantiated = Instantiate(prefab, stepPrefab.transform.GetChild(7), false);
                    instantiated.transform.rotation = obj.MiniatureRotation;
                    instantiated.transform.position += obj.MinaturePosition;
                    int scale = obj.MiniatureScale;
                    instantiated.transform.localScale = new Vector3(scale, scale, scale);
                    instantiated.layer = UILAYER;
                    foreach (Transform child in instantiated.transform)
                        child.gameObject.layer = UILAYER;
                    instantiated.transform.localPosition = new Vector3(0, 0, -10);
                    hasRequirement = false;
                }
                if (foundCharacterSkin && foundCollider && !getItems && !hasRequirement)
                    break;
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
                string[] names = itemGroup.getItemName().Split(":");
                string[] mandatoryCheck = names;
                if (names.Length > 1)
                {
                    mandatoryCheck = names[0].Split("Mandatory");
                }
                if (int.TryParse(mandatoryCheck[0], out int result))
                {
                    Node connectedNode = _allSteps[result].GetComponent<Node>();
                    Connection connection = nodeScript.ports[0].ConnectTo(connectedNode.ports[1]);
                    if (itemGroup.getItemName().Contains("Mandatory"))
                        connection.line.color = Color.red;
                }
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
        if (Input.GetMouseButtonUp(0))
        {
            foreach(Connection connection in UICSystemManager.Connections)
            {
                if(UICSystemManager.selectedElements.Contains(connection))
                {
                    if(connection.line.color == Color.red)
                        connection.line.color = connection.defaultColor;
                    else
                        connection.line.color = Color.red;
                }
            }
        }
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