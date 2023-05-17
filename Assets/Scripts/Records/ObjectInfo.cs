using System;
using UnityEngine;

[Serializable]
public class ObjectInfo
{

    [SerializeField]
    public string Name;
    [SerializeField]
    public PositionCoordinates Position;
    [SerializeField]
    public RotationCoordinates Rotation;

    public ObjectInfo(string name, PositionCoordinates pos, RotationCoordinates rot)
    {
        this.Name = name;
        this.Position = pos;
        this.Rotation = rot;
    }
}