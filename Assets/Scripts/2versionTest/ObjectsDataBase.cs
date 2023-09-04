using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDataBase : ScriptableObject
{
    public List<ObjectData> objectsDatabase;

    [ContextMenu("Set database")]
    public void SetObjects()
    {
        Dictionary<string,GameObject> alreadyInDatabase= new Dictionary<string,GameObject>();
        int i = 0;
        objectsDatabase = new List<ObjectData>();
        var foundItems = Resources.LoadAll("Prefabs",typeof(GameObject)).Cast<GameObject>().ToArray();
        foreach(var item in foundItems)
        {
            string objName = item.name.Split("(")[0];
            if (!alreadyInDatabase.ContainsKey(objName))
                alreadyInDatabase.Add(objName,item);
        }
        foreach(var item in alreadyInDatabase) 
        {
            ObjectData obj = new ObjectData(item.Key, i, Vector2Int.one,item.Value, ObjectData.ObjectTypes.Default, 25, new Quaternion(),new Vector3());
            objectsDatabase.Add(obj);
            i++;
        }
    }
}


[Serializable]
public class ObjectData
{
    //[HideInInspector] public enum ObjectTypes { Chair, Shelf, Table, Rug, Prop, Wall, Default, Floor };
    [HideInInspector] public enum ObjectTypes { Default, Floor, Prop, Structure, WallProp};

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public ObjectTypes Types { get; private set; }

    [field: SerializeField]
    public int MiniatureScale { get; private set; }

    [field: SerializeField]
    public Quaternion MiniatureRotation { get; private set; }

    [field: SerializeField]
    public Vector3 MinaturePosition { get; private set; }



    public ObjectData(string name, int iD, Vector2Int size, GameObject prefab, ObjectTypes type, int miniatureScale, Quaternion miniatureRotation, Vector3 MiniaturePosition) 
    {
        Name = name;
        ID = iD;
        Size = size;
        Prefab = prefab;
        Types = type;
        MiniatureScale = miniatureScale;
        MiniatureRotation = miniatureRotation;
        MinaturePosition = MiniaturePosition;

    }

}