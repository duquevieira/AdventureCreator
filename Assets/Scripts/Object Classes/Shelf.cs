using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Object
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
        AdjacentAvailablePositions[0] = false;
        AdjacentAvailablePositions[2] = false;
        AdjacentAvailablePositions[4] = false;
        AdjacentAvailablePositions[6] = false;
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
