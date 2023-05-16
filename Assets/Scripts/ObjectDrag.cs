using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{

    private Vector3 _offset;

    // Select an object pressing the mouse
    private void OnMouseDown()
    {
        _offset = transform.position - PlacementSystem.getMouseWorldPosition();
    }

    // Move an object dragging the mouse
    private void OnMouseDrag()
    {
        Vector3 pos = PlacementSystem.getMouseWorldPosition() + _offset;
        transform.position = PlacementSystem.Current.SnapCoordinateToGrid(pos);
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
    }
}
