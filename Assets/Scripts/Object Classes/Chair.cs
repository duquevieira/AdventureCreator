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

    public override Dictionary<ObjectTypes, int[]> getProbabilities()
    {
        return this.Probabilities;
    }

    /*public override int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return this.Probabilities.GetValueOrDefault(objectType);
    }*/

    public override void setProbabilities()
    {
        Probabilities = new Dictionary<ObjectTypes, int[]>();
        Probabilities.Add(ObjectTypes.Chair, new int[] { 0, 0, 90, 20, 90, 0, 90, 20, 90 });
        Probabilities.Add(ObjectTypes.Shelf, new int[] { 0, 0, 15, 0, 15, 0, 15, 0, 15 });
        Probabilities.Add(ObjectTypes.Table, new int[] { 0, 80, 0, 80, 0, 80, 0, 80, 0});
        Probabilities.Add(ObjectTypes.Rug, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Prop, new int[] { 5, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Wall, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
    }
}
