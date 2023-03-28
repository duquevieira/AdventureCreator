using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rug : Object
{
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
        AdjacentAvailablePositions2.Add(ObjectTypes.Chair, new int[] { 50, 100, 100, 100, 100, 100, 100, 100, 100 });
        AdjacentAvailablePositions2.Add(ObjectTypes.Shelf, new int[] { 25, 100, 100, 100, 100, 100, 100, 100, 100 });
        AdjacentAvailablePositions2.Add(ObjectTypes.Table, new int[] { 75, 100, 100, 100, 100, 100, 100, 100, 100 });
        AdjacentAvailablePositions2.Add(ObjectTypes.Rug, new int[] { 0, 100, 100, 100, 100, 100, 100, 100, 100 });
    }
}
