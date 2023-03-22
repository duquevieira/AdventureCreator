using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Object
{

    public override void setAttributes()
    {
        canBeAboveOf = new List<Object>();
        canBeAboveOf.Add(new Rug());
        //canBeBelowOf.Add(new Prop());
    }

    public override void setAdjacentAvailablePositions()
    {
        adjacentAvailablePositions = new bool[8];
        for (int i = 0; i < adjacentAvailablePositions.Length; i++)
        {
            adjacentAvailablePositions[i] = true;
        }
        adjacentAvailablePositions[0] = false;
        adjacentAvailablePositions[2] = false;
        adjacentAvailablePositions[4] = false;
        adjacentAvailablePositions[6] = false;
    }

    public override List<Object> getCanBeAboveOf()
    {
        return this.canBeAboveOf;
    }

    public override bool[] getAdjacentAvailablePositions()
    {
        return this.adjacentAvailablePositions;
    }
}
