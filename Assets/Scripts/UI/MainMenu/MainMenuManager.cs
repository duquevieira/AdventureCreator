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
    private float _angle;
    private int _index = 0;
    private bool hasChanged = false;
    private int _numObjects;

    void Start()
    {
        _numObjects = Objects.Length;
        _angle = (CIRCLE_ANGLE/_numObjects)*Mathf.Deg2Rad;
        for(int i = 0; i < _numObjects; i++) {
            Objects[i].transform.position = new Vector3(Center.position.x + RADIUS*Mathf.Cos(_angle*i+ANGLE_OFFSET), 0, Center.position.z + RADIUS*Mathf.Sin(_angle*i+ANGLE_OFFSET));
        }
    }

    void Update() {
        if(hasChanged) {
            var targetRotation = _index * _angle * Mathf.Rad2Deg;
            Quaternion targetQuarternion = Quaternion.Euler(0f, targetRotation, 0f);
            Center.rotation = Quaternion.RotateTowards(Center.rotation, targetQuarternion, 200 * Time.deltaTime);
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
        Objects[_index].GetComponent<MenuObject>().runFunction();
    }
}
