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
    protected static string COLLIDER_NAME = "StepCollider";
    protected static string OBTAINED_NAME = "ObtainedCollider";
    protected static string OBTAINED_SPOT = "ObtainedSpot";

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
            string name = hit.gameObject.name;
            if (name.Split(PARENTHESIS)[0].Equals(OBTAINED_NAME))
            {
                Transform current = hit.gameObject.transform.parent.GetChild(0);
                current.GetComponent<StepColourScript>().ToggleStepColor();
                noStep = false;
                setAsChild(current.transform.GetChild(5));
                break;
            }
            if (name.Split(PARENTHESIS)[0].Equals(OBTAINED_SPOT))
            {
                Transform current = hit.gameObject.transform.parent;
                current.GetComponent<StepColourScript>().ToggleStepColor();
                noStep = false;
                setAsChild(current.transform.GetChild(5));
                break;
            }
            if (name.Split(PARENTHESIS)[0].Equals(STEP_PREFAB_NAME))
            {
                Transform current = hit.gameObject.transform;
                noStep = false;
                if(_clone.GetComponent<LoopAnimationScript>() == null)
                {
                    setAsChild(current.GetChild(2));
                }
                else
                {
                    setAsChild(current.GetChild(4));
                }
                break;
            }
        }
        if (noStep)
            Destroy(_clone);
    }

    private void setAsChild (Transform parent)
    {
        if (parent.childCount != 0)
            Destroy(parent.GetChild(0).gameObject);
        _clone.transform.SetParent(parent, false);
        _clone.transform.localPosition = new Vector3(0, 0, -10);
    }
}