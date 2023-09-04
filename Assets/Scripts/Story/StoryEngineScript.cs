using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using static UnityEditor.Progress;

public class StoryEngineScript : MonoBehaviour
{

    [SerializeField]
    public Camera Camera;
    [SerializeField]
    public GameObject Player;

    [HideInInspector]
    public List<ItemGroup> StoryItems;
    [HideInInspector]
    public List<ItemGroup> InventoryItems;
    [HideInInspector]
    public List<StoryboardStep> Storyboard;
    [SerializeField]
    private InventoryItemsManager _itemsManager;


    void Awake()
    {
        ClearStoryElements();
        _itemsManager.setPlayerScript(Player.GetComponent<PlayerHandlerScript>());
        //TODO DELETE
        /*StoryboardStep step = new StoryboardStep(0, "SM_Prop_TableFootball_01", Vector3.zero);
        step.addAcquires(new ItemGroup("SM_Prop_TableFootball_01", 1));
        step.addAcquires(new ItemGroup("0", 1));
        Storyboard.Add(step);
        step = new StoryboardStep(1, "SM_Prop_Couch_02", Vector3.zero);
        step.addRequirement(new ItemGroup("SM_Prop_TableFootball_01", 1));
        step.addRequirement(new ItemGroup("0", 1));
        step.addAcquires(new ItemGroup("SM_Prop_Couch_02", 1));
        step.addAcquires(new ItemGroup("1", 1));
        Storyboard.Add(step);
        step = new StoryboardStep(1, "SM_Prop_Desk_01", Vector3.zero);
        step.addRequirement(new ItemGroup("SM_Prop_Couch_02", 1));
        step.addRequirement(new ItemGroup("1", 1));
        step.addAcquires(new ItemGroup("SM_Prop_Desk_01", 1));
        step.addAcquires(new ItemGroup("1", 1));
        Storyboard.Add(step);*/
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log(Time.realtimeSinceStartup + " " + Storyboard.Count);
            foreach (StoryboardStep step in Storyboard)
            {
                Debug.Log(Time.realtimeSinceStartup + "-----------------------------");
                Debug.Log(Time.realtimeSinceStartup + " ID " + step.getId());
                Debug.Log(Time.realtimeSinceStartup + " COLLIDER " + step.getColliderName());
                Debug.Log(Time.realtimeSinceStartup + " REQUIREMENTS");
                foreach (ItemGroup requirement in step.getRequirements())
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

    public bool ProcessEntry(Collider collider, string itemName)
    {
        string colliderName = collider.name;
        foreach (StoryboardStep iteratedStep in Storyboard)
            if (colliderName.Split("(")[0].Equals(iteratedStep.getColliderName()))
            {
                List<ItemGroup> requirements = iteratedStep.getRequirements();
                bool completable = true;
                bool itemIsNeeded = false;
                foreach (ItemGroup requirement in requirements)
                {
                    int amount = -1;
                    if (int.TryParse(requirement.getItemName(), out int number))
                    {
                        foreach (ItemGroup storyItem in StoryItems)
                            if (storyItem.getItemName() == requirement.getItemName())
                            {
                                amount = storyItem.getItemAmount();
                                break;
                            }
                    }
                    else
                    {
                        if(itemName == null)
                        {
                            completable = false;
                            break;
                        }
                        else if(requirement.getItemName().Equals(itemName))
                        {
                            itemIsNeeded = true;
                        }
                        foreach (ItemGroup inventoryItem in InventoryItems)
                            if (inventoryItem.getItemName() == requirement.getItemName())
                            {
                                amount = inventoryItem.getItemAmount();
                                break;
                            }
                    }
                    if(requirement.getItemAmount() > amount)
                    {
                        completable = false;
                        break;
                    }
                }
                if (itemName != null && !itemIsNeeded)
                    completable = false;
                if (completable)
                {
                    foreach (ItemGroup requirement in requirements)
                    {
                        foreach (ItemGroup storyItem in StoryItems)
                            if (storyItem.getItemName() == requirement.getItemName())
                            {
                                storyItem.removeItemAmount(requirement.getItemAmount());
                                if (storyItem.getItemAmount() == 0)
                                    StoryItems.Remove(storyItem);
                                break;
                            }
                        foreach (ItemGroup inventoryItem in InventoryItems)
                            if (inventoryItem.getItemName() == requirement.getItemName())
                            {
                                inventoryItem.removeItemAmount(requirement.getItemAmount());
                                //TODO DISPLAY REDUZIR QUANTIDADE VISIVEL NUMERO
                                //_itemsManager.UpdateAmount(inventoryItem);
                                if (inventoryItem.getItemAmount() == 0)
                                {
                                    _itemsManager.DeleteItem(inventoryItem.getItemName());
                                    InventoryItems.Remove(inventoryItem);
                                }
                                break;
                            }
                    }
                    foreach (ItemGroup acquires in iteratedStep.getAcquired())
                    {
                        bool newItem = true;
                        if (int.TryParse(acquires.getItemName(), out int number))
                        {
                            foreach (ItemGroup storyItem in StoryItems)
                            {
                                if (storyItem.getItemName() == acquires.getItemName())
                                {
                                    newItem = false;
                                    storyItem.addItemAmount(acquires.getItemAmount());
                                    break;
                                }
                            }
                            if (newItem)
                            {
                                ItemGroup item = new ItemGroup(acquires.getItemName(), acquires.getItemAmount());
                                StoryItems.Add(item);
                            }
                        }
                        else
                        {
                            foreach (ItemGroup inventoryItem in InventoryItems)
                            {
                                if (inventoryItem.getItemName() == acquires.getItemName())
                                {
                                    newItem = false;
                                    inventoryItem.addItemAmount(acquires.getItemAmount());
                                    break;
                                }
                            }
                            if (newItem)
                            {
                                ItemGroup item = new ItemGroup(acquires.getItemName(), acquires.getItemAmount());
                                InventoryItems.Add(item);
                                _itemsManager.AddItem(item);
                            }
                            Destroy(collider.gameObject);
                        }
                    }
                    return completable;
                }
            }
        return false;
    }

    public bool IsStoryStep(string colliderName)
    {
        foreach (StoryboardStep iteratedStep in Storyboard)
            if (colliderName.Split("(")[0].Equals(iteratedStep.getColliderName()))
                return true;
        return false; ;
    }

    public string getCharacterSkin()
    {
        //Santa, FWorker, MWorker, FAttendant, MAttendant, FClerk, MClerk
        //FGym, MGym, MHunter, FMusician, MMusician, FShopper, MShopper
        //EnglishCaptain, EnglishGovernor, EnglishSoldier, FPirate, FWench
        //Gentleman, GovernorsDaughter, PirateBlackbeard, PirateCaptain
        //PirateDeckHand, PirateFirstMate, PirateSeaman
        //Skeleton1, Skeleton2, Skeleton3
        return "MClerk";
    }

    public void ClearStoryElements()
    {
        StoryItems = new List<ItemGroup>();
        foreach (ItemGroup item in InventoryItems)
        {
            _itemsManager.DeleteItem(item.getItemName());
        }
        InventoryItems = new List<ItemGroup>();
        Storyboard = new List<StoryboardStep>();
    }
}