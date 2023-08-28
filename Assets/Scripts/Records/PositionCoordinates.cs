using System;
using UnityEngine;

[Serializable]
public class PositionCoordinates {
    [SerializeField]
    private float _row;

    [SerializeField]
    private float _column;

    [SerializeField]
    private float _height;


    public PositionCoordinates(float row, float col, float height)
    {
        _row = row;
        _column = col;
        _height = height;
    }

    public float getRow() {
        return _row;
    }

    public float getColumn() {
        return _column;
    }

    public float getHeight()
    {
        return _height;
    }

}