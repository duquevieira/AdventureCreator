using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static string GetPath(this Transform transform)
    {
        if (transform.parent == null)
        {
            return transform.name;
        }
        else
        {
            return transform.parent.GetPath() + "/" + transform.name;
        }
    }
}

