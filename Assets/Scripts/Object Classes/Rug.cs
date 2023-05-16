using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rug : Object
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
        Probabilities.Add(ObjectTypes.Chair, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 }); //85
        Probabilities.Add(ObjectTypes.Shelf, new int[] { 10, -1, -1, -1, -1, -1, -1, -1, -1 }); //10
        Probabilities.Add(ObjectTypes.Table, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 }); //95
        Probabilities.Add(ObjectTypes.Rug, new int[] { 0, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Prop, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Wall, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
    }
}
