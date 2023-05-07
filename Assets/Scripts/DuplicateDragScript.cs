using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        Ray ray = _uiCamera.ScreenPointToRay(Input.mousePosition);
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult hit in hits)
            if (hit.gameObject.name.Split(PARENTHESIS)[0].Equals(STEP_PREFAB_NAME))
                hit.gameObject.GetComponentsInChildren<InputField>()[0].SetTextWithoutNotify(_clone.name.Split(PARENTHESIS)[0]);
        Destroy(_clone);
    }
}