using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class StoryEngineScript : MonoBehaviour
{
    private static string DOOR = "Door";
    private static string ANIMATION_DOOR = "clicked";
    private static string KEY = "KeyExample";
    //private static string EXAMPLE = "ExampleItem";

    [SerializeField]
    public Camera Camera;
    [SerializeField]
    public GameObject Player;
    public Inventory Inventory;

    [HideInInspector]
    public List<ItemGroup> StoryItems;
    [HideInInspector]
    public Storyboard Storyboard;

    //TODO apagar
    [HideInInspector]
    public Canvas TestCanvas;
    [HideInInspector]
    public GameObject PrefabNewStep;
    private List<GameObject> _allSteps;
    public Text DialogPrefab;

    void Start()
    {
        StoryItems = new List<ItemGroup>();
        Storyboard = new Storyboard();
        //TODO apagar
        _allSteps = new List<GameObject>();
        StoryboardStep step = new StoryboardStep(Storyboard.getStorySteps().Count, "Lookout", "Talk to the pirate leaders for funding");
        step.addAcquires(new ItemGroup("Talk to the Pirate Leaders", 1));
        Storyboard.addStep(step);
        step = new StoryboardStep(Storyboard.getStorySteps().Count, "PirateLeaders", "We won't help you join LeChuck's crew");
        step.addRequirement(new ItemGroup("Talk to the Pirate Leaders", 1));
        step.addAcquires(new ItemGroup("Talk to Elaine", 1));
        Storyboard.addStep(step);
        step = new StoryboardStep(Storyboard.getStorySteps().Count, "Elaine", "Hey GuyBrush");
        step.addRequirement(new ItemGroup("Talk to Elaine", 1));
        step.addAcquires(new ItemGroup("Talk to Prisoner", 1));
        Storyboard.addStep(step);
        step = new StoryboardStep(Storyboard.getStorySteps().Count, "Prisoner", "Can you help me out?");
        step.addRequirement(new ItemGroup("Talk to Prisoner", 1));
        step.addAcquires(new ItemGroup("Talk to Locksmith", 1));
        Storyboard.addStep(step);
        step = new StoryboardStep(Storyboard.getStorySteps().Count, "Locksmith", "Here you go boss");
        step.addRequirement(new ItemGroup("Talk to Locksmith", 1));
        step.addAcquires(new ItemGroup("Key", 1));
        Storyboard.addStep(step);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == DOOR &&
                Vector3.Distance(Player.transform.position, hit.collider.transform.position) < 3 &&
                Inventory.GetQuantity(KEY) > 0)
                {
                    Animator doorAnimator = hit.collider.GetComponent<Animator>();
                    MMFeedbacks clickFeedBack = hit.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MMFeedbacks>();
                    doorAnimator.SetBool(ANIMATION_DOOR, true);
                    clickFeedBack?.PlayFeedbacks();
                    StartCoroutine(ExecuteAfter(3, doorAnimator, ANIMATION_DOOR));
                }
            }
        }
        //DEBUGGING ITEMS
        if (Input.GetKeyDown(KeyCode.T)) {
            Debug.Log(Time.realtimeSinceStartup + " ----------------------------------------");
            Debug.Log(Time.realtimeSinceStartup + " Story Items:");
            foreach (ItemGroup storyItem in StoryItems)
            {
                Debug.Log(Time.realtimeSinceStartup + " Story item name:" + storyItem.getItemName());
                Debug.Log(Time.realtimeSinceStartup + " Story item amount:" + storyItem.getItemAmount());
            }
        }
        //DEBUGGING STORY
        if (Input.GetKeyDown(KeyCode.S)) {
            /*foreach (StoryboardStep step in Storyboard.getStorySteps())
            {
                GameObject newStep = Instantiate(PrefabNewStep, TestCanvas.transform);
                newStep.transform.SetParent(TestCanvas.transform, false);
                _allSteps.Add(newStep);
                InputField[] inputFields = newStep.GetComponentsInChildren<InputField>();
                inputFields[0].SetTextWithoutNotify(step.getColliderName());
                inputFields[1].SetTextWithoutNotify(step.getDialog());
                List<ItemGroup> requirements = step.getRequirements();
                List<ItemGroup> acquires = step.getAcquired();
                if (requirements.Count > 0)
                    inputFields[2].SetTextWithoutNotify(requirements[0].getItemAmount() + " " + requirements[0].getItemName());
                if (acquires.Count > 0)
                    inputFields[3].SetTextWithoutNotify(acquires[0].getItemAmount() + " " + acquires[0].getItemName());
            }*/
            List<StoryboardStep> storySteps = Storyboard.getStorySteps();
            for (int i = storySteps.Count; i > 0; i--)
            {
                StoryboardStep step = storySteps[i-1];
                GameObject newStep = Instantiate(PrefabNewStep, TestCanvas.transform);
                newStep.transform.SetParent(TestCanvas.transform, false);
                _allSteps.Add(newStep);
                InputField[] inputFields = newStep.GetComponentsInChildren<InputField>();
                inputFields[0].SetTextWithoutNotify(step.getColliderName());
                inputFields[1].SetTextWithoutNotify(step.getDialog());
                List<ItemGroup> requirements = step.getRequirements();
                List<ItemGroup> acquires = step.getAcquired();
                if (requirements.Count > 0)
                    inputFields[2].SetTextWithoutNotify(requirements[0].getItemAmount() + " " + requirements[0].getItemName());
                if (acquires.Count > 0)
                    inputFields[3].SetTextWithoutNotify(acquires[0].getItemAmount() + " " + acquires[0].getItemName());
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (GameObject step in _allSteps)
            {
                Destroy(step);
            }
        }
    }

    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
    }

    //TODO apagar
    IEnumerator APAGAR(float time, Text apagar)
    {
        yield return new WaitForSeconds(time);
        Destroy(apagar);
    }

    public void ProcessEntry(string colliderName)
    {
        foreach (StoryboardStep iteratedStep in Storyboard.getStorySteps())
            if (colliderName.Equals(iteratedStep.getColliderName()))
            {
                List<ItemGroup> requirements = iteratedStep.getRequirements();
                bool completable = true;
                foreach(ItemGroup requirement in requirements)
                {
                    int storyAmount = -1;
                    foreach(ItemGroup storyItem in StoryItems)
                        if(storyItem.getItemName() == requirement.getItemName())
                        {
                            storyAmount = storyItem.getItemAmount();
                            break;
                        }
                    int inventoryAmount = Inventory.GetQuantity(requirement.getItemName());
                    int requiredAmount = requirement.getItemAmount();
                    if (inventoryAmount < requiredAmount && storyAmount < requiredAmount)
                    {
                        completable = false;
                        break;
                    }
                }
                if (completable)
                {
                    //TODO apagar
                    Text newDialog = Instantiate(DialogPrefab, TestCanvas.transform);
                    newDialog.transform.SetParent(TestCanvas.transform, false);
                    newDialog.text = iteratedStep.getDialog();
                    StartCoroutine(APAGAR(3f, newDialog));
                    foreach (ItemGroup requirement in requirements)
                    {
                        foreach (ItemGroup storyItem in StoryItems)
                            if (storyItem.getItemName() == requirement.getItemName())
                            {
                                storyItem.removeItemAmount(requirement.getItemAmount());
                                if(storyItem.getItemAmount() == 0)
                                    StoryItems.Remove(storyItem);
                                break;
                            }
                        Inventory.RemoveItemByID(requirement.getItemName(), requirement.getItemAmount());
                    }
                    foreach (ItemGroup acquires in iteratedStep.getAcquired())
                    {
                        bool newItem = true;
                        foreach (ItemGroup storyItem in StoryItems)
                        {
                            if (storyItem.getItemName() == acquires.getItemName())
                            {
                                newItem = false;
                                storyItem.addItemAmount(acquires.getItemAmount());
                                break;
                            }
                        }
                        if(newItem)
                        {
                            ItemGroup item = new ItemGroup(acquires.getItemName(), acquires.getItemAmount());
                            StoryItems.Add(item);
                        }
                    }
                }
            }
    }

    public string getCharacterSkin()
    {
        //Santa, FWorker, MWorker, FAttendant, MAttendant, FClerk, MClerk
        //FGym, MGym, MHunter, FMusician, MMusician, FShopper, MShopper
        //EnglishCaptain, EnglishGovernor, EnglishSoldier, FPirate, FWench
        //Gentleman, GovernorsDaughter, PirateBlackbeard, PirateCaptain
        //PirateDeckHand, PirateFirstMate, PirateSeaman
        //Skeleton1, Skeleton2, Skeleton3
        return "PirateBlackbeard";
    }
}
