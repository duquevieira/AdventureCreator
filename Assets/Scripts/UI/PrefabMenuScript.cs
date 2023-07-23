using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class PrefabMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;
    /*[SerializeField]
    private GameObject _panelPrefab;*/
    [SerializeField]
    private GameObject _menuSlotPrefab;
    [SerializeField] ObjectsDataBase _database;
    [HideInInspector]
    public List<GameObject> AllPrefabs;//substituir
    //public List<List<GameObject>> AllPrefabs;

    private static int UILAYER = 5;

    private static string[] foldersToSearch = {"Assets/Resources/TrainingSet"/*//, "Assets/Resources/Items"
     ,"Assets/PolygonOffice/Prefabs/Characters", "Assets/PolygonShops/Prefabs/Characters", "Assets/PolygonPirates/Prefabs/Characters", 
     "Assets/PolygonCity/Prefabs/Characters", "Assets/PolygonAncientEmpire/Prefabs/Characters"*/};

    public void Start()
    {
        foreach (var obj in _database.objectsDatabase)
        {
            int scale = obj.MiniatureScale;
            var menuSlot = Instantiate(_menuSlotPrefab, _panel.transform);
            var instantiated = Instantiate(obj.Prefab, menuSlot.transform);
            if (obj.Types == ObjectData.ObjectTypes.Structure)
                instantiated.transform.Rotate(0, 90, 0);
            if (obj.Types == ObjectData.ObjectTypes.Floor) 
                instantiated.transform.Rotate(-90,0,0);
            instantiated.layer = UILAYER;
            instantiated.transform.localScale = new Vector3(scale, scale, scale);
            foreach (Transform child in instantiated.transform)
                child.gameObject.layer = UILAYER;
        }
    }
}
        
        
        //substituir
        /*AllPrefabs = PrefabNameLoader.GetAssets<GameObject>(foldersToSearch, "t:prefab");
        foreach (GameObject prefab in AllPrefabs)
        {
            var menuSlot = Instantiate(_menuSlotPrefab, _panel.transform);
            var instantiated = Instantiate(prefab, menuSlot.transform);
            instantiated.layer = UILAYER;
          
            instantiated.transform.localScale = new Vector3(25, 25, 25);
            foreach (Transform child in instantiated.transform)
                child.gameObject.layer = UILAYER;
        }*/


        /*AllPrefabs = new List<List<GameObject>>();
        int i = 0;
        foreach(string folder in foldersToSearch)
        {
            string[] folderName = { folder };
            AllPrefabs.Add(PrefabNameLoader.GetAssets<GameObject>(folderName, "t:prefab"));
            var panelPrefab = Instantiate(_panelPrefab, _panel.transform);
            TextMeshProUGUI title = panelPrefab.GetComponent<TextMeshProUGUI>();
            string[] section = folder.Split("/");
            title.SetText(section[section.Length-1]);
            foreach (GameObject prefab in AllPrefabs[i++])
            {
                var menuSlot = Instantiate(_menuSlotPrefab, panelPrefab.transform);
                var instantiated = Instantiate(prefab, menuSlot.transform);
                instantiated.layer = UILAYER;
                //TODO
                instantiated.transform.localScale = new Vector3(25, 25, 25);
                foreach (Transform child in instantiated.transform)
                    child.gameObject.layer = UILAYER;
            }
        }*/