using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Table : Object
{
    public override int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return this.ProbabilitiesBasedOnAdjacentObject.GetValueOrDefault(objectType);
    }

    public override Dictionary<ObjectTypes, int[]> getProbabilitiesBasedOnAdjacentObject()
    {
        return this.ProbabilitiesBasedOnAdjacentObject;
    }

    public override void setProbabilitiesBasedOnAdjacentObject()
    {
        ProbabilitiesBasedOnAdjacentObject = new Dictionary<ObjectTypes, int[]>();
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Chair, new int[]{ 0, 80, 0, 80, 0, 80, 0, 80, 0 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Shelf, new int[] { 0, 0, 10, 0, 10, 0, 10, 0, 10 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Table, new int[] { 0, 10, 5, 30 , 5, 10, 5, 30, 5 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Rug, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Prop, new int[] { 40, -1, -1, -1, -1, -1, -1, -1, -1 });
    }

}
