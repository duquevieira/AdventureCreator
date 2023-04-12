using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Object;

public class Wall : Object
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
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Chair, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Shelf, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Table, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Rug, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Prop, new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 });
        ProbabilitiesBasedOnAdjacentObject.Add(ObjectTypes.Wall, new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100 });

    }
}
