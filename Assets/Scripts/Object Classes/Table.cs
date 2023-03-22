using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Object
{
    public override void setAttributes()
    {
        canBeAboveOf = new List<Object>();
        canBeAboveOf.Add(new Rug());
        //canBeBelowOf.Add(new Prop());
    }

    public override void setAdjacentAvailablePositions()
    {
        for (int i = 0; i < adjacentAvailablePositions.Length; i++)
        {
            adjacentAvailablePositions[i] = true;
        }
        adjacentAvailablePositions[1] = false;
        adjacentAvailablePositions[3] = false;
        adjacentAvailablePositions[5] = false;
        adjacentAvailablePositions[7] = false;
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
