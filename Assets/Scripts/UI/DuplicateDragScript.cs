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
    protected static string PARENTHESIS = "(";
    protected static string COLLIDER_SPOT = "ColliderSpot";
    protected static string OBTAINED_SPOT = "ObtainedSpot";
    protected static string ANIMATION_SPOT = "AnimationSpot";

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
            Transform current = hit.gameObject.transform.parent;
            if (hit.gameObject.name.Split(PARENTHESIS)[0].Equals(COLLIDER_SPOT))
            {
                if (_clone.GetComponent<LoopAnimationScript>() != null)
                {
                    Transform colliderTransform = current.gameObject.transform.GetChild(2);
                    if (colliderTransform.childCount != 0 && colliderTransform.GetChild(0).gameObject.name.Contains("Character_"))
                    {
                        Animator NPCAnimator = colliderTransform.GetChild(0).gameObject.GetComponent<Animator>();
                        NPCAnimator.SetInteger("targetAnimation", int.Parse(_clone.gameObject.name.Split(" ")[1]));
                    }
                }
                else
                {
                    setAsChild(current.transform.GetChild(2));
                    noStep = false;
                }
                break;
            }
            if (hit.gameObject.name.Split(PARENTHESIS)[0].Equals(ANIMATION_SPOT) && _clone.GetComponent<LoopAnimationScript>() != null)
            {
                setAsChild(current.gameObject.transform.GetChild(4));
                noStep = false;
                break;
            }
            if (hit.gameObject.name.Split(PARENTHESIS)[0].Equals(OBTAINED_SPOT) && _clone.GetComponent<LoopAnimationScript>() == null)
            {
                current.GetComponent<StepHandlerScript>().ToggleStepItem();
                setAsChild(current.transform.GetChild(5));
                noStep = false;
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