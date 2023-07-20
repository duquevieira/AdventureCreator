using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCoords : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private PlacementSystemV2 _placementSys;
    public event Action OnClicked, OnExit, Rotate;
    private Vector3 _lastPos;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if (Input.GetMouseButtonDown(1))
            OnExit?.Invoke();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _placementSys.RotateStructure();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    public Vector3 getMouseWorldPosition()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }*/
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            _lastPos = hit.point;
        }
        return _lastPos;
    }
}
