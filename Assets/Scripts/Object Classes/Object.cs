using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [HideInInspector] public List<Object> canBeAboveOf;
    [HideInInspector] public List<Object> canBeBelowOf;
    [HideInInspector] public bool[] adjacentAvailablePositions;

    public virtual List<Object> getCanBeAboveOf() { return canBeAboveOf; }
    public virtual List<Object> getCanBeBelowOf() { return canBeBelowOf; }
    public virtual bool[] getAdjacentAvailablePositions() { return adjacentAvailablePositions; }
    public virtual void setAdjacentAvailablePositions()
    {
        for (int i = 0; i < adjacentAvailablePositions.Length; i++)
        {
            adjacentAvailablePositions[i] = true;
        }
    }
    public virtual void setAttributes() { }

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


