using System;
using UnityEngine;

[Serializable]
public class ObjectInfo
{

    [SerializeField]
    public string name;
    [SerializeField]
    public PositionCoordinates pos;
    [SerializeField]
    public RotationCoordinates rot;

    public ObjectInfo(string name, PositionCoordinates pos, RotationCoordinates rot)
    {
        this.name = name;
        this.pos = pos;
        this.rot = rot;
    }
}