using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chair : Object
{
    /*public override void setAttributes()
    {
        CanBeAboveOf = new List<Object>
        {
            new Rug()
        };
    }*/

    /*public override void setAdjacentAvailablePositions()
    {
        AdjacentAvailablePositions = new bool[8];
        for (int i = 0; i < AdjacentAvailablePositions.Length; i++)
        {
            AdjacentAvailablePositions[i] = true;
        }
    }*/

    /* public override List<Object> getCanBeAboveOf()
     {
         return this.CanBeAboveOf;
     }*/

    /*public override bool[] getAdjacentAvailablePositions()
    {
        return this.AdjacentAvailablePositions;
    }*/

    public override Dictionary<ObjectTypes, int[]> getAdjacentAvailablePositions2()
    {
        return this.AdjacentAvailablePositions2;
    }

    public override int[] getAdjacentAvailablePositionsProbabilities(ObjectTypes objectType)
    {
        return this.AdjacentAvailablePositions2.GetValueOrDefault(objectType);
    }

    public override void setAdjacentAvailablePositions2()
    { 
        AdjacentAvailablePositions2 = new Dictionary<ObjectTypes, int[]>();
        AdjacentAvailablePositions2.Add(ObjectTypes.Chair, new int[] { 0, 0, 90, 20, 90, 0, 90, 20, 90 });
        AdjacentAvailablePositions2.Add(ObjectTypes.Shelf, new int[] { 0, 15, 3, 0, 6, 15, 3, 0, 6 });
        AdjacentAvailablePositions2.Add(ObjectTypes.Table, new int[] { 0, 50, 0, 50, 0, 50, 0, 50, 0});
        AdjacentAvailablePositions2.Add(ObjectTypes.Rug, new int[] { 50, 100, 100, 100, 100, 100, 100, 100, 100 });
    }
}
