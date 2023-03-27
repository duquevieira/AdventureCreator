using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using MoreMountains.TopDownEngine;
using static UnityEditor.Progress;

public class StoryEngineScript : MonoBehaviour
{
    //String constants
    private static string DOOR = "Door";
    private static string ANIMATION_DOOR = "clicked";
    private static string KEY = "KeyExample";
    private static string EXAMPLE = "ExampleItem";

    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Inventory inventory;

    private List<ItemGroup> storyItems;
    private Storyboard storyboard;

    void Start()
    {
        storyboard = new Storyboard();
        storyItems = new List<ItemGroup>();
        
        //int id, string colliderName, string dialog
        StoryboardStep event0 = new StoryboardStep(0, "Pot", "pot dialog");
        event0.addRequirement(new ItemGroup(EXAMPLE, 1));
        event0.addAcquires(new ItemGroup("id1", 10));

        StoryboardStep event1 = new StoryboardStep(1, "TallGrass", "grass dialog");
        event1.addRequirement(new ItemGroup("id1", 1));
        event1.addRequirement(new ItemGroup(EXAMPLE, 5));
        event1.addAcquires(new ItemGroup("id2", 10));

        storyboard.addStep(event0);
        storyboard.addStep(event1);

        //Santa, FWorker, MWorker, FAttendant, MAttendant, FClerk, MClerk, FGym, MGym, MHunter, FMusician, MMusician, FShopper, MShopper
        player.GetComponent<PlayerHandlerScript>().Setup(camera, player, "FMusician");
    }
    void Update()
    {
        //Check if Player Left-clicked a door in range and in possession of the required key
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == DOOR &&
                Vector3.Distance(player.transform.position, hit.collider.transform.position) < 3 &&
                inventory.GetQuantity(KEY) > 0)
                {
                    //Open the door
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
            foreach (ItemGroup storyItem in storyItems)
            {
                Debug.Log(Time.realtimeSinceStartup + " Story item name:" + storyItem.getItemName());
                Debug.Log(Time.realtimeSinceStartup + " Story item amount:" + storyItem.getItemAmount());
            }
        }
        //DEBUGGING STORY
        if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log(Time.realtimeSinceStartup + " ----------------------------------------");
            Debug.Log(Time.realtimeSinceStartup + " Story Steps:");
            foreach (StoryboardStep step in storyboard.getStorySteps())
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

    //Auxiliary function to update an Animator after the animation is over
    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
    }

    public void ProcessEntry(Collider other)
    {
        //Iterate all story steps to find the one that is being triggered by this collider
        foreach (StoryboardStep iteratedStep in storyboard.getStorySteps())
        {
            if (other.gameObject.name.Equals(iteratedStep.getColliderName()))
            {
                List<ItemGroup> requirements = iteratedStep.getRequirements();
                bool completable = true;
                //Iterate all item requirements of the event to check if the player satisfies every one of them
                foreach(ItemGroup requirement in requirements)
                {
                    int storyAmount = -1;
                    //Find the player's required story item amount
                    foreach(ItemGroup storyItem in storyItems)
                    {
                        if(storyItem.getItemName() == requirement.getItemName())
                        {
                            storyAmount = storyItem.getItemAmount();
                            break;
                        }
                    }
                    int inventoryAmount = inventory.GetQuantity(requirement.getItemName());
                    int requiredAmount = requirement.getItemAmount();
                    //Check if the user does not have enough items to satisfy this steps's requirements
                    if (inventoryAmount < requiredAmount && storyAmount < requiredAmount)
                    {
                        completable = false;
                        break;
                    }
                }
                //If all requirements were met
                if (completable)
                {
                    //Iterate all item requirements of the event to consume them
                    foreach (ItemGroup requirement in requirements)
                    {
                        //Find the player's required story item to decrease it's amount
                        foreach (ItemGroup storyItem in storyItems)
                        {
                            if (storyItem.getItemName() == requirement.getItemName())
                            {
                                storyItem.removeItemAmount(requirement.getItemAmount());
                                break;
                            }
                        }
                        //Consume the item present in the user's inventory
                        inventory.RemoveItemByID(requirement.getItemName(), requirement.getItemAmount());
                    }
                    //We can now add the story items this step unlocks
                    foreach(ItemGroup acquires in iteratedStep.getAcquired())
                    {
                        bool newItem = true;
                        //Check if we want to increase the amount of an existing item or add a new one to the list
                        foreach(ItemGroup storyItem in storyItems)
                        {
                            if(storyItem.getItemName() == acquires.getItemName())
                            {
                                newItem = false;
                                storyItem.addItemAmount(acquires.getItemAmount());
                                break;
                            }
                        }
                        if(newItem)
                        {
                            storyItems.Add(acquires);
                        }
                    }
                }
                break;
            }
        }
    }
}
