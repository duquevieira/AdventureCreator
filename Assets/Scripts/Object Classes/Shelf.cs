using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : PlaceableObject
{
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
        Probabilities.Add(ObjectTypes.Chair, new int[] { 0, 0, 15, 0, 15, 0, 15, 0, 15 });
        Probabilities.Add(ObjectTypes.Shelf, new int[] { 0, 10, 0, 80, 0, 10, 0, 80, 0 });
        Probabilities.Add(ObjectTypes.Table, new int[] { 0, 0, 10, 0, 10, 0, 10, 0, 10 });
        Probabilities.Add(ObjectTypes.Rug, new int[] { 10, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Prop, new int[] { 0, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Wall, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
    }

}
