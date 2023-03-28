using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Table : Object
{
    /*public override void setAttributes()
    {
        CanBeAboveOf = new List<Object>
        {
            new Rug()
        };
        //canBeBelowOf.Add(new Prop());
    }*/

    /*public override void setAdjacentAvailablePositions()
    {
        AdjacentAvailablePositions = new bool[8];
        for (int i = 0; i < AdjacentAvailablePositions.Length; i++)
        {
            AdjacentAvailablePositions[i] = true;
        }
        AdjacentAvailablePositions[1] = false;
        AdjacentAvailablePositions[3] = false;
        AdjacentAvailablePositions[5] = false;
        AdjacentAvailablePositions[7] = false;
    }*/

    /*public override List<Object> getCanBeAboveOf()
    {
        return this.CanBeAboveOf;
    }*/

    /*public override bool[] getAdjacentAvailablePositions()
    {
        return this.AdjacentAvailablePositions;
    }*/

    public override int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return this.ProbabilitiesBasedOnAdjacentObject.GetValueOrDefault(objectType);
    }

    public override Dictionary<ObjectTypes, int[]> getProbabilitiesBasedOnAdjacentObject()
    {
        return this.ProbabilitiesBasedOnAdjacentObject;
    }

    public override void setProbabilitiesBasedOnAdjacentObject()
    {
        ProbabilitiesBasedOnAdjacentObject = new Dictionary<ObjectTypes, int[]>();
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Chair, new int[]{ 0, 50, 0, 50, 0, 50, 0, 50, 0 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Shelf, new int[] { 0, 25, 10, 0, 10, 25, 10, 0, 10 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Table, new int[] { 0, 60, 10, 75, 5, 60, 10, 75, 5 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Rug, new int[] { 75, 100, 100, 100, 100, 100, 100, 100, 100 });
    }

}
