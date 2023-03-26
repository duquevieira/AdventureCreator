using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Object
{
    public override void setAttributes()
    {
        CanBeAboveOf = new List<Object>
        {
            new Rug()
        };
    }

    public override void setAdjacentAvailablePositions()
    {
        AdjacentAvailablePositions = new bool[8];
        for (int i = 0; i < AdjacentAvailablePositions.Length; i++)
        {
            AdjacentAvailablePositions[i] = true;
        }
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
