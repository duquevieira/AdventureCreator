using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Object
{
    public override void setAttributes()
    {
        CanBeAboveOf = new List<Object>
        {
            new Rug()
        };
        //canBeBelowOf.Add(new Prop());
    }

    public override void setAdjacentAvailablePositions()
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
    }

    public override List<Object> getCanBeAboveOf()
    {
        return this.CanBeAboveOf;
    }

    public override bool[] getAdjacentAvailablePositions()
    {
        return this.AdjacentAvailablePositions;
    }

}
