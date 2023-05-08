using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class StoryEngineScript : MonoBehaviour
{

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
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach(StoryboardStep step in Storyboard.getStorySteps())
            {
                Debug.Log(Time.realtimeSinceStartup + "-----------------------------");
                Debug.Log(Time.realtimeSinceStartup + " ID " + step.getId());
                Debug.Log(Time.realtimeSinceStartup + " COLLIDER " + step.getColliderName());
                Debug.Log(Time.realtimeSinceStartup + " REQUIREMENTS");
                foreach(ItemGroup requirement in step.getRequirements())
                {
                    Debug.Log(Time.realtimeSinceStartup + " " + requirement.getItemName() + " " + requirement.getItemAmount());
                }
                Debug.Log(Time.realtimeSinceStartup + " ACQUIRES");
                foreach (ItemGroup acquires in step.getAcquired())
                {
                    Debug.Log(Time.realtimeSinceStartup + " " + acquires.getItemName() + " " + acquires.getItemAmount());
                }
            }
        }
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
        //Santa, FWorker, MWorker, FAttendant, MAttendant, FClerk, MClerk
        //FGym, MGym, MHunter, FMusician, MMusician, FShopper, MShopper
        //EnglishCaptain, EnglishGovernor, EnglishSoldier, FPirate, FWench
        //Gentleman, GovernorsDaughter, PirateBlackbeard, PirateCaptain
        //PirateDeckHand, PirateFirstMate, PirateSeaman
        //Skeleton1, Skeleton2, Skeleton3
        return "PirateBlackbeard";
    }
}
