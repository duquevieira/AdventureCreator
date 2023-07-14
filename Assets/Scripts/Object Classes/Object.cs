using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object: MonoBehaviour
{
    [HideInInspector] public Dictionary<ObjectTypes, int[]> Probabilities;
    //[HideInInspector] public enum AdjacentPositions { Top, TopLeft,TopRight, Right,Left,On,BottomLeft,Bottom,BottomRight };
    [HideInInspector] public enum ObjectTypes { Chair, Shelf, Table, Rug, Prop, Wall, Default};

    public virtual Dictionary<ObjectTypes, int[]> getProbabilities()
    {
        return Probabilities;
    }
    /*public virtual int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return Probabilities.GetValueOrDefault(objectType);
    }*/

    public virtual void setProbabilities()
    {
        Probabilities = new Dictionary<ObjectTypes, int[]>();
    }

}


