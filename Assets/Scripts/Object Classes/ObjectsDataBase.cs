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
        var foundItems = Resources.LoadAll("TrainingSet",typeof(GameObject)).Cast<GameObject>().ToArray();
        foreach(var item in foundItems)
        {
            string objName = item.name.Split("(")[0];
            if (!alreadyInDatabase.ContainsKey(objName))
                alreadyInDatabase.Add(objName,item);
        }
        foreach(var item in alreadyInDatabase) 
        {
            ObjectData obj = new ObjectData(item.Key, i, Vector2Int.one,item.Value, ObjectData.ObjectTypes.Default);
            objectsDatabase.Add(obj);
            i++;
        }
    }
}


[Serializable]
public class ObjectData
{
    [HideInInspector] public enum ObjectTypes { Chair, Shelf, Table, Rug, Prop, Wall, Default };

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public ObjectTypes Types { get; private set; }

    public ObjectData(string name, int iD, Vector2Int size, GameObject prefab, ObjectTypes type) 
    {
        Name = name;
        ID = iD;
        Size = size;
        Prefab = prefab;
        Types = type;
    }

}