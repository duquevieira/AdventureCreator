using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class GameObjectExtensions
{
    public static ObjectInfo GetProp(this GameObject gameObject)
    {
        Vector3 position = gameObject.transform.position;
        Quaternion rotation = gameObject.transform.rotation;
        PositionCoordinates pos = new PositionCoordinates(position.x, position.z);
        RotationCoordinates rot = new RotationCoordinates(rotation);
        return new ObjectInfo(gameObject.name, pos, rot);
    }
}






