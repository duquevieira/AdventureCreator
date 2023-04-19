using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using MoreMountains.TopDownEngine;

public class StoryEngineScript : MonoBehaviour
{
    //String constants
    private static string DOOR = "Door";
    private static string ANIMATION_DOOR = "clicked";
    private static string PLAYER_INVENTORY = "CharacterInventory";
    private static string KEY = "KeyExample";

    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private GameObject player;

    private List<StoryboardEvent> story;
    private List<StoryboardEvent> availableEvents;

    void Start()
    {
        story = new List<StoryboardEvent>();
        availableEvents = new List<StoryboardEvent>();
        //int id, string actionName, string colliderName, string inventoryItemName,
        //int inventoryItemChange, string dialog, List<int> nextEvents

        StoryboardEvent event0 = new StoryboardEvent(0, "actionName", "Pot", "inventoryItemName", 0, "dialog", new List<int>());
        event0.addNextEvent(1);
        StoryboardEvent event1 = new StoryboardEvent(1, "actionName", "TallGrass", "inventoryItemName", 0, "dialog", new List<int>());
        event1.addNextEvent(2);
        event1.addNextEvent(3);
        StoryboardEvent event2 = new StoryboardEvent(2, "actionName", "Pot", "inventoryItemName", 0, "dialog", new List<int>());
        event2.addNextEvent(4);
        StoryboardEvent event3 = new StoryboardEvent(3, "actionName", "TallGrass", "inventoryItemName", 0, "dialog", new List<int>());
        event3.addNextEvent(5);
        StoryboardEvent event4 = new StoryboardEvent(4, "actionName", "Pot", "inventoryItemName", 0, "dialog", new List<int>());
        StoryboardEvent event5 = new StoryboardEvent(5, "actionName", "TallGrass", "inventoryItemName", 0, "dialog", new List<int>());

        story.Add(event0); story.Add(event1); story.Add(event2); story.Add(event3); story.Add(event4); story.Add(event5);
        availableEvents.Add(event0);

        //Pot, TallGrass
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
                GameObject.Find(PLAYER_INVENTORY).GetComponent<Inventory>().GetQuantity(KEY) > 0)
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (StoryboardEvent iteratedEvent in availableEvents)
            {
                Debug.Log(Time.realtimeSinceStartup + " ID: " + iteratedEvent.getId() + " Collider: " + iteratedEvent.getColliderName());
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (StoryboardEvent iteratedEvent in story)
            {
                Debug.Log(Time.realtimeSinceStartup + " ID: " + iteratedEvent.getId() + " Collider: " + iteratedEvent.getColliderName());
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
        foreach(StoryboardEvent iteratedEvent in availableEvents)
        {
            if (other.gameObject.name.Equals(iteratedEvent.getColliderName()))
            {
                Debug.Log(Time.realtimeSinceStartup + " Evento correto");
                foreach(int i in iteratedEvent.getNextEvents())
                {
                    availableEvents.Add(story[i]);
                }
                availableEvents.Remove(iteratedEvent);
                if (availableEvents.Count == 0)
                {
                    Debug.Log(Time.realtimeSinceStartup + " Historia Acabou");
                }
                break;
            }
        }
    }
}
