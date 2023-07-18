using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryItemsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private GameObject _itemSlotPrefab;

    [SerializeField]
    private StoryEngineScript _storyEngineScript;

    private List<GameObject> _displayedItems;

    private static int UILAYER = 5;
    private static string PARENTHESIS = "(";

    public void setStoryEngineScript(StoryEngineScript storyEngineScript)
    {
        _storyEngineScript = storyEngineScript;
    }

    private static string[] foldersToSearch = {"Assets/Resources/Interactables"/*
     ,"Assets/PolygonOffice/Prefabs/Characters", "Assets/PolygonShops/Prefabs/Characters", "Assets/PolygonPirates/Prefabs/Characters", 
     "Assets/PolygonCity/Prefabs/Characters", "Assets/PolygonAncientEmpire/Prefabs/Characters"*/
    };

    public void Start()
    {
        _displayedItems = new List<GameObject>();
    }

    public void DeleteItem(string itemName)
    {
        foreach(GameObject itemSlot in _displayedItems)
            if(itemSlot.transform.GetChild(0).name.Equals(itemName))
            {
                _displayedItems.Remove(itemSlot);
                Destroy(itemSlot);
                break;
            }
    }

    public void AddItem(ItemGroup item)
    {

        //TODO Resources.Load para o asset quando estiver no nosso folder e nao nas pastas do polygon
        string[] lookingFor = {"Assets/Resources/TrainingSet"};
        List<GameObject> prefabs = PrefabNameLoader.GetAssets<GameObject>(lookingFor, "t:prefab");
        foreach(GameObject prefab in prefabs)
            if(prefab.name.Split("(")[0].Equals(item.getItemName()))
            {
                var itemSlot = Instantiate(_itemSlotPrefab, _panel.transform);
                itemSlot.name = prefab.name.Split("(")[0];
                _displayedItems.Add(itemSlot);
                itemSlot.GetComponent<InventoryItemDragScript>().SetStoryEngineScript(_storyEngineScript);
                var instantiated = Instantiate(prefab, itemSlot.transform);
                instantiated.name = prefab.name.Split(PARENTHESIS)[0];
                instantiated.layer = UILAYER;
                //TODO
                instantiated.transform.localScale = new Vector3(25, 25, 25);
                foreach (Transform child in instantiated.transform)
                    child.gameObject.layer = UILAYER;
                break;
            }
    }
}

