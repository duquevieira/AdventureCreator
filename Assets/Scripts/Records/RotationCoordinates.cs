using System;
using UnityEngine;

[Serializable]
public class RotationCoordinates {

    [SerializeField]
    private Direction _direction;

    public RotationCoordinates(Quaternion quart) {
        float yRotation = quart.eulerAngles.y;

        if(yRotation < 45f || yRotation > 315f) {
            _direction = Direction.North;
        }
        else if(yRotation >= 45 && yRotation < 135f) {
            _direction = Direction.East;
        }
        else if(yRotation >= 135f && yRotation < 225f) {
            _direction = Direction.South;
        } else {
            _direction = Direction.West;
        }
    }

    public Direction GetDirection() {
        return _direction;
    }

}