using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PrefabMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private GameObject _menuSlotPrefab;

    private static int UILAYER = 5;

    private static string[] foldersToSearch = {"Assets/Resources/AndreUI_test"/*//, "Assets/Resources/Items"
     ,"Assets/PolygonOffice/Prefabs/Characters", "Assets/PolygonShops/Prefabs/Characters", "Assets/PolygonPirates/Prefabs/Characters", 
     "Assets/PolygonCity/Prefabs/Characters", "Assets/PolygonAncientEmpire/Prefabs/Characters"*/};
    public void Start()
    {
        List<GameObject> allPrefabs = GetAssets<GameObject>(foldersToSearch, "t:prefab");
        foreach (GameObject prefab in allPrefabs)
        {
            var menuSlot = Instantiate(_menuSlotPrefab, _panel.transform);
            var instantiated = Instantiate(prefab, menuSlot.transform);
            instantiated.layer = UILAYER;
            //TODO
            instantiated.transform.localScale = new Vector3(25, 25, 25);
            foreach (Transform child in instantiated.transform)
                child.gameObject.layer = UILAYER;
        }
    }

    //Adapted from http://answers.unity.com/answers/1698175/view.html
    private static List<T> GetAssets<T>(string[] _foldersToSearch, string _filter) where T : UnityEngine.Object
    {
        string[] guids = AssetDatabase.FindAssets(_filter, _foldersToSearch);
        List<T> assets = new List<T>();
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }
        return assets;
    }
}
