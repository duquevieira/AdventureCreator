using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Table : PlaceableObject
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
        Probabilities.Add(ObjectTypes.Chair, new int[]{ 0, 80, 0, 80, 0, 80, 0, 80, 0 });
        Probabilities.Add(ObjectTypes.Shelf, new int[] { 0, 0, 10, 0, 10, 0, 10, 0, 10 });
        Probabilities.Add(ObjectTypes.Table, new int[] { 0, 10, 5, 30 , 5, 10, 5, 30, 5 });
        Probabilities.Add(ObjectTypes.Rug, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Prop, new int[] { 40, -1, -1, -1, -1, -1, -1, -1, -1 });
        Probabilities.Add(ObjectTypes.Wall, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
    }

}
