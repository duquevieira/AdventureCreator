using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DuplicateDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    protected Camera _uiCamera;
    protected GameObject _clone;

    protected static string CAMERA_NAME = "UICamera";
    protected static string STEP_PREFAB_NAME = "Step";
    protected static string PARENTHESIS = "(";

    void Start()
    {
        _uiCamera = GameObject.Find(transform.root.name).transform.Find(CAMERA_NAME).GetComponent<Camera>();
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _clone = Instantiate(gameObject.transform.GetChild(0).gameObject, gameObject.transform);
        _clone.name = gameObject.transform.GetChild(0).gameObject.name.Split(PARENTHESIS)[0];
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _clone.transform.position = _uiCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _clone.transform.position.z));
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {   
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        bool noStep = true;
        foreach (RaycastResult hit in hits)
        {
            GameObject current = hit.gameObject;
            if (current.name.Split(PARENTHESIS)[0].Equals(STEP_PREFAB_NAME))
            {
                noStep = false;
                Transform colliderSpot = current.transform.GetChild(2);
                if (colliderSpot.childCount != 0)
                    Destroy(colliderSpot.GetChild(0).gameObject);
                _clone.transform.SetParent(colliderSpot, false);
                _clone.transform.localPosition = Vector3.zero;
            }
        }
        if (noStep)
            Destroy(_clone);
    }
}