using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rug : Object
{
    public override Dictionary<ObjectTypes, int[]> getProbabilitiesBasedOnAdjacentObject()
    {
        return this.ProbabilitiesBasedOnAdjacentObject;
    }

    public override int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return this.ProbabilitiesBasedOnAdjacentObject.GetValueOrDefault(objectType);
    }

    public override void setProbabilitiesBasedOnAdjacentObject()
    {
        ProbabilitiesBasedOnAdjacentObject = new Dictionary<ObjectTypes, int[]>();
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Chair, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 }); //85
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Shelf, new int[] { 10, -1, -1, -1, -1, -1, -1, -1, -1 }); //10
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Table, new int[] { 100, -1, -1, -1, -1, -1, -1, -1, -1 }); //95
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Rug, new int[] { 0, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Prop, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Wall, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
    }
}
