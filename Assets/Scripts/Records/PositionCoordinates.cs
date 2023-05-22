using System;
using UnityEngine;

[Serializable]
public class PositionCoordinates {
    [SerializeField]
    private float _row;

    [SerializeField]
    private float _column;


    public PositionCoordinates(float row, float col) {
        _row = row;
        _column = col;
    }

    public float getRow() {
        return _row;
    }

    public float getColumn() {
        return _column;
    }

}