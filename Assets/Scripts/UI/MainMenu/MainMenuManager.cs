using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private const int RADIUS = 75;
    private const int CIRCLE_ANGLE = 360;
    private const float ANGLE_OFFSET = -90*Mathf.Deg2Rad;
    public GameObject[] Objects;
    public Transform Center;
    private Vector3[] _positions;
    private float _angle;
    private int _index = 0;
    private bool hasChanged = false;
    private int _numObjects;
    private float animationDuration = 1f;
    private float animationTimer = 0f;

    void Start()
    {
        _numObjects = Objects.Length;
        _positions = new Vector3[_numObjects];
        _angle = (CIRCLE_ANGLE/_numObjects)*Mathf.Deg2Rad;
        var frontPosition = Center.position - new Vector3(0,0,50);
        for(int i = 0; i < _numObjects; i++) {
            Objects[i].transform.position = new Vector3(frontPosition.x + RADIUS*Mathf.Cos(_angle*i+ANGLE_OFFSET), 0, frontPosition.z + RADIUS*Mathf.Sin(_angle*i+ANGLE_OFFSET));
        }
        for(int i = 0; i < _numObjects; i++) {
            _positions[i] = Objects[i].transform.position;
        }
    }

    void Update() {
        if(hasChanged) {
            hasChanged = false;

        }
    }

    public void Left() {
        Debug.Log("Left!");
        if(_index == 0) _index = _numObjects-1;
        else _index--;
        hasChanged = true;
    }

    public void Right() {
        Debug.Log("Right!");
        if(_index == _numObjects-1) _index = 0;
        else _index++;
        hasChanged = true;
    }

    public void Interact() {
        Debug.Log("Interacted");
    }
}
