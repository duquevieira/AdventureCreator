using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;

public class StoryEngineScript : MonoBehaviour
{

    [SerializeField]
    public Camera Camera;
    [SerializeField]
    public GameObject Player;

    [HideInInspector]
    public string playerSkin = "Character_Pirate_Gentleman_01";

    [HideInInspector]
    public List<ItemGroup> StoryItems;
    [HideInInspector]
    public List<ItemGroup> InventoryItems;
    [HideInInspector]
    public List<StoryboardStep> Storyboard;
    [SerializeField]
    private InventoryItemsManager _itemsManager;
    [SerializeField]
    private Image _dialogBox;

    void Awake()
    {
        ClearStoryElements();
        _itemsManager.setPlayerScript(Player.GetComponent<PlayerHandlerScript>());
    }

    void Start()
    {
        foreach(StoryboardStep step in Storyboard)
        {
            foreach(ItemGroup requirement in step.getRequirements())
            {
                if(requirement.getItemName().Contains("Start"))
                {
                    StoryItems.Add(requirement);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (ItemGroup storyItem in StoryItems)
            {
                Debug.Log(Time.realtimeSinceStartup + " Name " + storyItem.getItemName() + " Amount " + storyItem.getItemAmount());
            }
            foreach (ItemGroup inventoryItem in InventoryItems)
            {
                Debug.Log(Time.realtimeSinceStartup + " Name " + inventoryItem.getItemName() + " Amount " + inventoryItem.getItemAmount());
            }
            Debug.Log(Time.realtimeSinceStartup + " " + Storyboard.Count);
            foreach (StoryboardStep step in Storyboard)
            {
                Debug.Log(Time.realtimeSinceStartup + "-----------------------------");
                Debug.Log(Time.realtimeSinceStartup + " ID " + step.getId());
                Debug.Log(Time.realtimeSinceStartup + " COLLIDER " + step.getColliderName());
                Debug.Log(Time.realtimeSinceStartup + " DIALOG " + step.getDialog());
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

    public int ProcessEntry(Collider collider, string itemName)
    {
        string colliderName = collider.name;
        foreach (StoryboardStep iteratedStep in Storyboard)
            if (colliderName.Split("(")[0].Equals(iteratedStep.getColliderName()))
            {
                List<ItemGroup> requirements = iteratedStep.getRequirements();
                bool completable = false;
                List<string> dependentItems = new List<string>();
                foreach (ItemGroup requirement in requirements)
                {
                    int amount = -1;
                    string[] names = requirement.getItemName().Split(":");
                    if (names[0].Contains("Start") || int.TryParse(names[0], out int number))
                    {
                        foreach (ItemGroup storyItem in StoryItems) {
                            if (storyItem.getItemName() == requirement.getItemName())
                            {
                                amount = storyItem.getItemAmount();
                                break;
                            }
                        }
                    }
                    else
                    {
                        dependentItems.Add(requirement.getItemName());
                        if (itemName != null && requirement.getItemName().Equals(itemName))
                        {
                            foreach (ItemGroup inventoryItem in InventoryItems)
                                if (inventoryItem.getItemName() == itemName)
                                {
                                    amount = inventoryItem.getItemAmount();
                                    break;
                                }
                        }

                    }

                    if (requirement.getItemName().Contains("Mandatory"))
                    {
                        completable = false;
                        foreach (ItemGroup item in StoryItems)
                            if(item.getItemName().Equals(requirement.getItemName()))
                                completable = true;
                        if(!completable)
                            goto breakLoop;
                    }
                    if(requirement.getItemAmount() <= amount)
                    {
                        completable = true;
                    }
                }
                if(completable && dependentItems.Count > 0 && InventoryItems.Count > 0)
                {
                    foreach (ItemGroup inventoryItem in InventoryItems)
                    {
                        foreach(string dependentItem in dependentItems)
                        {
                            if(inventoryItem.getItemName().Equals(dependentItem))
                            {
                                completable = false;
                                if(dependentItem.Equals(itemName))
                                {
                                    completable = true;
                                    goto breakLoop;
                                }
                            }
                        }
                    }
                }
            breakLoop:
                if (completable)
                {
                    bool valid = false;
                    for (int i = 0; i < iteratedStep.getDialog().Length && !valid; i++)
                    {
                        char c = iteratedStep.getDialog()[i];
                        if(char.IsLetterOrDigit(c) || char.IsPunctuation(c))
                        {
                            valid = true;
                        }
                    }
                    if (valid)
                    {
                        _dialogBox.gameObject.SetActive(true);
                        StartCoroutine(TypeSentence(iteratedStep.getDialog()));
                    }
                    if (colliderName.Contains("Character_"))
                    {
                        Animator NPCAnimator = collider.GetComponent<Animator>();
                        NPCAnimator.SetInteger("targetAnimation", iteratedStep.getNPCAnimation());
                    }
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
                        string[] itemNames = acquires.getItemName().Split(":");
                        if (itemNames.Length > 1)
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
                            if(acquires.getItemName() == iteratedStep.getColliderName())
                                Destroy(collider.gameObject);
                        }
                    }
                    Animator animator = collider.gameObject.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetBool("On", !animator.GetBool("On"));
                    }
                    return iteratedStep.getAnimation();
                }
            }
        return -1;
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
        return playerSkin;
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

    IEnumerator TypeSentence (string sentence)
    {
        TextMeshProUGUI dialogText = _dialogBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dialogText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }
}