using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler
{

    private Vector3 _lastPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastPosition = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Input.mousePosition - _lastPosition)/25;
        _lastPosition = Input.mousePosition;
    }
}
