using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableObject;

public class Wall : PlaceableObject
{
    /*public override int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return this.Probabilities.GetValueOrDefault(objectType);
    }*/

    public override Dictionary<ObjectTypes, int[]> getProbabilities()
    {
        return this.Probabilities;
    }

    public override void setProbabilities()
    {
        Probabilities = new Dictionary<ObjectTypes, int[]>();
        Probabilities.Add(ObjectTypes.Chair, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Shelf, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Table, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Rug, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Prop, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Wall, new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100 });

    }
}
