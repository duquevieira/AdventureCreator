using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;

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

    void Start()
    {
        StoryItems = new List<ItemGroup>();
        Storyboard = new Storyboard();
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
            Debug.Log(Time.realtimeSinceStartup + " ----------------------------------------");
            Debug.Log(Time.realtimeSinceStartup + " Story Steps:");
            foreach (StoryboardStep step in Storyboard.getStorySteps())
            {
                Debug.Log(Time.realtimeSinceStartup + " Story step id:" + step.getId());
                Debug.Log(Time.realtimeSinceStartup + " Story step collider name:" + step.getColliderName());
                Debug.Log(Time.realtimeSinceStartup + " Story step dialog:" + step.getDialog());
                Debug.Log(Time.realtimeSinceStartup + " Requirements:");
                foreach (ItemGroup requirement in step.getRequirements())
                {
                    Debug.Log(Time.realtimeSinceStartup + " Story step item name:" + requirement.getItemName());
                    Debug.Log(Time.realtimeSinceStartup + " Story step item amount:" + requirement.getItemAmount());
                }
                Debug.Log(Time.realtimeSinceStartup + " Acquired:");
                foreach (ItemGroup acquires in step.getAcquired())
                {
                    Debug.Log(Time.realtimeSinceStartup + " Story step item name:" + acquires.getItemName());
                    Debug.Log(Time.realtimeSinceStartup + " Story step item amount:" + acquires.getItemAmount());
                }
            }
        }
    }

    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
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
        //Santa, FWorker, MWorker, FAttendant, MAttendant, FClerk, MClerk,
        //FGym, MGym, MHunter, FMusician, MMusician, FShopper, MShopper
        return "FAttendant";
    }
}
