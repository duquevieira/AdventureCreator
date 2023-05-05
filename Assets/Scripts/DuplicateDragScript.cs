using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DuplicateDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Vector3 _lastPosition;
    private GameObject clone;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastPosition = Input.mousePosition;
        clone = Instantiate(gameObject, gameObject.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        clone.transform.position += (Input.mousePosition - _lastPosition) / 25;
        _lastPosition = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (Transform child in clone.transform)
        {
            Debug.Log(child.name.Split("(")[0]);
        }
        /*Camera uiCamera = GameObject.Find(transform.root.name).transform.Find("UICamera").GetComponent<Camera>();
        Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.name);
        }*/
        
        Debug.Log(eventData);
        Destroy(clone);
    }
}