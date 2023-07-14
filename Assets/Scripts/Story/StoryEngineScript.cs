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


    void Start()
    {
        ClearStoryElements();
        _itemsManager.setStoryEngineScript(this);
        //TODO APAGAR APENAS PARA TESTAR
        /*StoryboardStep step = new StoryboardStep(0, "Lookout", Vector3.zero);
        step.addAcquires(new ItemGroup("Fish", 1));
        step.addAcquires(new ItemGroup("0", 1));
        Storyboard.Add(step);
        step = new StoryboardStep(1, "SM_Veh_Boat_Small_01_Hull 3", Vector3.zero);
        step.addRequirement(new ItemGroup("Fish", 1));
        step.addRequirement(new ItemGroup("0", 1));
        step.addAcquires(new ItemGroup("Shark", 1));
        step.addAcquires(new ItemGroup("1", 1));
        Storyboard.Add(step);
        step = new StoryboardStep(1, "PirateLeaders", Vector3.zero);
        step.addRequirement(new ItemGroup("Shark", 1));
        step.addRequirement(new ItemGroup("1", 1));
        step.addAcquires(new ItemGroup("Character_Skeleton_03", 1));
        Storyboard.Add(step);*/
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (StoryboardStep step in Storyboard)
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
                Debug.Log(Time.realtimeSinceStartup + " COORDINATES " + step.getStepCoordinates());
            }
        }
    }

    public void ProcessEntry(string colliderName)
    {
        foreach (StoryboardStep iteratedStep in Storyboard)
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
                    int inventoryAmount = -1;
                    foreach (ItemGroup inventoryItem in InventoryItems)
                        if (inventoryItem.getItemName() == requirement.getItemName())
                        {
                            inventoryAmount = inventoryItem.getItemAmount();
                            break;
                        }
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
                        if(int.TryParse(acquires.getItemName(), out int number))
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
                            if (newItem)
                            {
                                ItemGroup item = new ItemGroup(acquires.getItemName(), acquires.getItemAmount());
                                StoryItems.Add(item);
                            }
                        }
                        else
                        {
                            string prefabName = acquires.getItemName().Split("@Step@")[1];
                            foreach(string aux in acquires.getItemName().Split("@Step@"))//TODO DELETE
                            {
                                Debug.Log(Time.realtimeSinceStartup + " " + aux);
                            }
                            bool newItem = true;
                            foreach (ItemGroup inventoryItem in InventoryItems)
                            {
                                if (inventoryItem.getItemName() == prefabName)
                                {
                                    newItem = false;
                                    inventoryItem.addItemAmount(acquires.getItemAmount());
                                    break;
                                }
                            }
                            if (newItem)
                            {
                                ItemGroup item = new ItemGroup(prefabName, acquires.getItemAmount());
                                InventoryItems.Add(item);
                                _itemsManager.AddItem(item);
                            }
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

    public void ClearStoryElements()
    {
        StoryItems = new List<ItemGroup>();
        InventoryItems = new List<ItemGroup>();
        Storyboard = new List<StoryboardStep>();
    }
}
