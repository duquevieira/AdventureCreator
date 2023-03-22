using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Object
{
    public override void setAttributes()
    {
        canBeAboveOf = new List<Object>();
        canBeAboveOf.Add(new Rug());
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
