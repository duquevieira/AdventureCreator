using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object: MonoBehaviour
{
    [HideInInspector] public List<Object> CanBeAboveOf;
    [HideInInspector] public List<Object> CanBeBelowOf;
    [HideInInspector] public bool[] AdjacentAvailablePositions;
    [HideInInspector] public Dictionary<ObjectTypes, int[]> AdjacentAvailablePositions2;
    //[HideInInspector] public enum AdjacentPositions { Top, TopLeft,TopRight, Right,Left,On,BottomLeft,Bottom,BottomRight };
    [HideInInspector] public enum ObjectTypes { Chair, Shelf, Table, Rug, Default};
    //public virtual List<Object> getCanBeAboveOf() { return CanBeAboveOf; }
    //public virtual List<Object> getCanBeBelowOf() { return CanBeBelowOf; }
    //public virtual bool[] getAdjacentAvailablePositions() { return AdjacentAvailablePositions; }
    /*public virtual void setAdjacentAvailablePositions()
    {
        for (int i = 0; i < AdjacentAvailablePositions.Length; i++)
        {
            AdjacentAvailablePositions[i] = true;
        }
    }*/

    public virtual Dictionary<ObjectTypes, int[]> getAdjacentAvailablePositions2()
    {
        return AdjacentAvailablePositions2;
    }
    public virtual int[] getAdjacentAvailablePositionsProbabilities(ObjectTypes objectType)
    {
        return AdjacentAvailablePositions2.GetValueOrDefault(objectType);
    }

    public virtual void setAdjacentAvailablePositions2()
    {
        AdjacentAvailablePositions2 = new Dictionary<ObjectTypes, int[]>();
    }
    //public virtual void setAttributes() { }

}

/*public class Table : Object
{
    public Table()
    {
        type = objectTypes.Table;
        canBeAboveOf[0] = objectTypes.Rug;
        canBeBelowOf[0] = objectTypes.Prop;
        adjacentAvailablePositions[1] = false;
        adjacentAvailablePositions[3] = false;
        adjacentAvailablePositions[5] = false;
        adjacentAvailablePositions[7] = false;
    }
}*/


