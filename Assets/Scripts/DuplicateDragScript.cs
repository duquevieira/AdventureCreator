using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DuplicateDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Camera _uiCamera;
    private GameObject _clone;

    private static string CAMERA_NAME = "UICamera";
    private static string STEP_PREFAB_NAME = "Step";
    private static string PARENTHESIS = "(";

    void Start()
    {
        _uiCamera = GameObject.Find(transform.root.name).transform.Find(CAMERA_NAME).GetComponent<Camera>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _clone = Instantiate(gameObject.transform.GetChild(0).gameObject, gameObject.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _clone.transform.position = _uiCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _clone.transform.position.z));
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        bool noStep = true;
        foreach (RaycastResult hit in hits)
            if (hit.gameObject.name.Split(PARENTHESIS)[0].Equals(STEP_PREFAB_NAME))
            {
                noStep = false;
                if(hit.gameObject.transform.GetChild(2).childCount != 0)
                    Destroy(hit.gameObject.transform.GetChild(2).GetChild(0).gameObject);
                _clone.transform.SetParent(hit.gameObject.transform.GetChild(2), false);
                _clone.transform.localPosition = Vector3.zero;
            }
        if (noStep)
            Destroy(_clone);


        RaycastHit res;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out res))
        {
            Debug.Log(Time.realtimeSinceStartup + " " + _clone.name + " " + res.transform.name);
        }
    }
}