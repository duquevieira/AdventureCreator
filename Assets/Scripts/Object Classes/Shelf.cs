using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Object
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
        AdjacentAvailablePositions[0] = false;
        AdjacentAvailablePositions[2] = false;
        AdjacentAvailablePositions[4] = false;
        AdjacentAvailablePositions[6] = false;
    }*/

    public override Dictionary<ObjectTypes, int[]> getProbabilitiesBasedOnAdjacentObject()
    {
        return this.ProbabilitiesBasedOnAdjacentObject;
    }

    public override int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return this.ProbabilitiesBasedOnAdjacentObject.GetValueOrDefault(objectType);
    }

    public override void setProbabilitiesBasedOnAdjacentObject()
    {
        ProbabilitiesBasedOnAdjacentObject = new Dictionary<ObjectTypes, int[]>();
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Chair, new int[] { 0, 15, 3, 0, 6, 15, 3, 0, 6 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Shelf, new int[] { 0, 10, 0, 80, 0, 10, 0, 80, 0 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Table, new int[] { 0, 25, 10, 0, 10, 25, 10, 0, 10 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Rug, new int[] { 25, 100, 100, 100, 100, 100, 100, 100, 100 });
    }

    /*public override List<Object> getCanBeAboveOf()
    {
        return this.CanBeAboveOf;
    }*/

    /*public override bool[] getAdjacentAvailablePositions()
    {
        return this.AdjacentAvailablePositions;
    }*/
}
