using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object: MonoBehaviour
{
    [HideInInspector] public Dictionary<ObjectTypes, int[]> ProbabilitiesBasedOnAdjacentObject;
    //[HideInInspector] public enum AdjacentPositions { Top, TopLeft,TopRight, Right,Left,On,BottomLeft,Bottom,BottomRight };
    [HideInInspector] public enum ObjectTypes { Chair, Shelf, Table, Rug, Prop, Default};
  

    public virtual Dictionary<ObjectTypes, int[]> getProbabilitiesBasedOnAdjacentObject()
    {
        return ProbabilitiesBasedOnAdjacentObject;
    }
    public virtual int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return ProbabilitiesBasedOnAdjacentObject.GetValueOrDefault(objectType);
    }

    public virtual void setProbabilitiesBasedOnAdjacentObject()
    {
        ProbabilitiesBasedOnAdjacentObject = new Dictionary<ObjectTypes, int[]>();
    }

}


